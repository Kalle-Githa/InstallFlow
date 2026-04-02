using InstallFlow.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddControllers();

// ===== 4. OpenAPI (krävs för Scalar) =====
builder.Services.AddOpenApi();

// ===== 5. DI-registreringar =====
// Här kommer vi lägga till våra services och repositories senare, t.ex:
// builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
// builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

// ===== Middleware-pipeline =====
// Ordningen här spelar roll!

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