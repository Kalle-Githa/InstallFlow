using InstallFlow.Core.Interfaces;
using InstallFlow.Core.Services;
using InstallFlow.Data;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Enums;
using InstallFlow.Data.Interfaces;
using InstallFlow.Data.Repos;
using InstallFlow.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;



var builder = WebApplication.CreateBuilder(args);
var baseUrl = builder.Configuration["APP_BASE_URL"] ?? "https://localhost:8000";

// ===== 1. EF Core =====
// Registrerar vår DbContext och talar om vilken databas vi ska använda.
// GetConnectionString("DefaultConnection") hämtar strängen från appsettings.json men i detta fall via user-sercrets
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "ConnectionStrings:DefaultConnection saknas. Sätt via user-secrets (lokalt) eller env-variabel (Docker/Azure).");

builder.Services.AddDbContext<InstallFlowDbContext>(options =>
    options.UseSqlServer(connectionString));


// ===== 2. JWT-autentisering =====
// Hämtar JWT-inställningar från appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException(
        "Jwt:Key saknas. Sätt via user-secrets (lokalt) eller env-variabel (Docker/Azure).");

var jwtIssuer = jwtSettings["Issuer"]
    ?? throw new InvalidOperationException(
        "Jwt:Issuer saknas. Lägg till i appsettings.json eller sätt via env-variabel.");

var jwtAudience = jwtSettings["Audience"]
    ?? throw new InvalidOperationException(
        "Jwt:Audience saknas. Lägg till i appsettings.json eller sätt via env-variabel.");

var key = Encoding.UTF8.GetBytes(jwtKey);


builder.Services.AddAuthentication(options =>
{
    // Säger att default-schemat för autentisering är JWT
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Här konfigurerar vi vad som ska valideras i varje JWT-token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ===== 3. Controllers + JSON =====
builder.Services.AddControllers()
    .AddNewtonsoftJson();

// ===== 4. OpenAPI (krävs för Scalar) =====
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Servers = new List<OpenApiServer>
    {
        new OpenApiServer { Url = baseUrl } // TODO: Förklara mer
    };

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

            document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Klistra in din JWT-token här (utan 'Bearer')"
            };

            var requirement = new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
            };

            foreach (var operation in document.Paths.Values.SelectMany(p => p.Operations!))
            {
                operation.Value.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Value.Security.Add(requirement);
            }

            return Task.CompletedTask;
        });
});


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    // Läs dessa två headers från proxyn
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto   // ← rätt protokoll
                             | ForwardedHeaders.XForwardedFor;    // ← rätt IP

    // Lita på ALLA proxies, oavsett IP
    options.KnownNetworks.Clear();  // lita inte bara på lokala nätverk
    options.KnownProxies.Clear();   // lita inte bara på kända IP-adresser
});




// ===== 5. DI-registreringar =====
// Här kommer vi lägga till våra services och repositories senare, t.ex:
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAssignmentRepo, AssignmentRepo>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IJobRepo, JobRepo>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ICartRepo, CartRepo>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

// ===== Application Insights (endast om connection string finns) =====
var appInsightsConnection = builder.Configuration["ApplicationInsights:ConnectionString"];

if (!string.IsNullOrWhiteSpace(appInsightsConnection))
{
    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = appInsightsConnection;
    });
}


var app = builder.Build();


// MÅSTE ligga först i pipelinen så att alla efterföljande middlewares
// ser rätt scheme (https) och rätt klient-IP. Använder konfigurationen
// från DI ovan — därför inga argument här.
app.UseForwardedHeaders(); // ← applicerar headers så resten av appen ser rätt värden


// ===== Middleware-pipeline =====
// Ordningen här spelar roll!
app.UseMiddleware<ExceptionMiddleware>();  // ← ÖVERST — fångar allt nedanför

// Scalar API-dokumentation — endast i Development, döljs i Production
// (ASPNETCORE_ENVIRONMENT=Development → visas även i Azure för den här appen)
if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Servers = new List<ScalarServer> { new ScalarServer(baseUrl) };
    });
}

// HTTPS-redirect — aldrig inne i en container
// Azure/Docker terminerar TLS utanför containern
var runningInContainer =
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

if (!runningInContainer)
{
    app.UseHttpsRedirection();
}

// Authentication MÅSTE komma före Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Default seed user för dev — byt lösenord innan produktion
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<InstallFlowDbContext>();

    if (!context.Users.Any())
    {
        context.Users.Add(new User
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        });

        context.SaveChanges();
    }
}




app.Run();