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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "ConnectionStrings:DefaultConnection saknas. Sätt via user-secrets (lokalt) eller env-variabel (Docker/Azure).");

builder.Services.AddDbContext<InstallFlowDbContext>(options =>
    options.UseSqlServer(connectionString));

// ===== 2. JWT-autentisering =====
var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException(
        "Jwt:Key saknas. Sätt via user-secrets (lokalt) eller env-variabel (Docker/Azure).");

var jwtIssuer = jwtSettings["Issuer"]
    ?? throw new InvalidOperationException("Jwt:Issuer saknas.");

var jwtAudience = jwtSettings["Audience"]
    ?? throw new InvalidOperationException("Jwt:Audience saknas.");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
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
// Newtonsoft krävs för JSON Patch-stöd
builder.Services.AddControllers()
    .AddNewtonsoftJson();

// ===== 4. OpenAPI (krävs för Scalar) =====
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // Sätter rätt server-URL i OpenAPI-dokumentet (viktigt för Scalar i Azure)
        document.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = baseUrl }
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

// ===== 5. Forwarded Headers =====
// Behövs i container bakom proxy (t.ex. Azure App Service) så att appen
// ser rätt protokoll (https) och rätt klient-IP istället för proxyns.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto
                             | ForwardedHeaders.XForwardedFor;

    // Töm standardlistorna så att alla proxies accepteras
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

// ===== 6. DI-registreringar =====
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

// ===== 7. Application Insights =====
// Registreras bara om connection string finns (slipper krascha lokalt utan Azure-koppling)
var appInsightsConnection = builder.Configuration["ApplicationInsights:ConnectionString"];

if (!string.IsNullOrWhiteSpace(appInsightsConnection))
{
    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = appInsightsConnection;
    });
}

var app = builder.Build();

// Måste ligga först så att resten av middleware ser rätt scheme och IP
app.UseForwardedHeaders();

// Fångar alla exceptions från middleware nedanför och mappar till HTTP-statuskoder
app.UseMiddleware<ExceptionMiddleware>();

// Scalar/OpenAPI exponeras i alla miljöer utom Production
if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Servers = new List<ScalarServer> { new ScalarServer(baseUrl) };
    });
}

// Skippa HTTPS-redirect i container — Azure/Docker terminerar TLS innan trafiken når appen
var runningInContainer =
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

if (!runningInContainer)
{
    app.UseHttpsRedirection();
}

// Authentication måste komma före Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seedar en admin-användare första gången appen startar mot en tom databas.
// Kör i alla miljöer — byt lösenord direkt efter första deploy till produktion.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InstallFlowDbContext>();

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