using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstallFlow.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _authService = authService;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto.UserName, dto.Password);

        if (token == null)
            return Unauthorized(new { error = "Fel användarnamn eller lösenord." });
        _logger.LogInformation("Användaren {Username} loggade in", dto.UserName);

        var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

        return Ok(new LoginResponseDto
        {
            AccessToken = token,
            TokenType = "Bearer",
            ExpiresIn = expiresInMinutes * 60
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var user = await _authService.CreateUserAsync(dto);
        _logger.LogInformation("Ny användare skapad: {Username} med roll {Role}", dto.Username, dto.Role);

        return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, user);
    }
}