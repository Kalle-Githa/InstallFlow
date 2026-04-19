using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InstallFlow.Core.Interfaces;
using InstallFlow.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InstallFlow.Core.Services;

public class AuthService : IAuthService
{
    private readonly InstallFlowDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(InstallFlowDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        // Steg 1: Hämta användaren
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        // Temporär debug
        Console.WriteLine($"Söker efter: '{username}'");
        Console.WriteLine($"Hittade användare: {user?.Username ?? "NULL"}");
        Console.WriteLine($"Hash i DB: {user?.PasswordHash}");
        var verify = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        Console.WriteLine($"BCrypt.Verify result: {verify}");
        Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("admin123"));
        
        

        if (user == null) return null;

        // Steg 2: Verifiera lösenordet
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        // Steg 3: Bygg claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
           
        };

        // Steg 4: Signera och generera token
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: signingCreds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}