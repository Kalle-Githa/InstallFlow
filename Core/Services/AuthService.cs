using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Auth;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InstallFlow.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepo _userRepo;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepo userRepo, IConfiguration configuration)
    {
        _userRepo = userRepo;
        _configuration = configuration;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        // Steg 1: Hämta användaren
        var user = await _userRepo.GetByUsernameAsync(username);
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
        var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
            signingCredentials: signingCreds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var existing = await _userRepo.GetByUsernameAsync(dto.Username);
        if (existing != null)
            throw new InvalidOperationException($"Användarnamnet '{dto.Username}' är redan taget.");

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepo.CreateAsync(user);
        await _userRepo.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }
}