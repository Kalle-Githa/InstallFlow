using InstallFlow.Core.Interfaces;
using InstallFlow.Core.Services;
using InstallFlow.Data;
using InstallFlow.Data.Interfaces;
using InstallFlow.Data.Repos;
using InstallFlow.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// ===== 1. EF Core =====
// Registrerar vår DbContext och talar om vilken databas vi ska använda.
// GetConnectionString("DefaultConnection") hämtar strängen från appsettings.json.
builder.Services.AddDbContext<InstallFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== 2. JWT-autentisering =====
// Hämtar JWT-inställningar från appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
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


var app = builder.Build();




// ===== Middleware-pipeline =====
// Ordningen här spelar roll!
app.UseMiddleware<ExceptionMiddleware>();  // ← ÖVERST — fångar allt nedanför

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Scalar ersätter Swagger — snyggar API-dokumentation
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Authentication MÅSTE komma före Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();