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

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto.UserName, dto.Password);

        if (token == null)
            return Unauthorized(new { error = "Fel användarnamn eller lösenord." });

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
        return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, user);
    }
}