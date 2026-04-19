using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace InstallFlow.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
   private readonly IAuthService _authService;

   public AuthController(IAuthService authService)
   {
      _authService = authService;
   }

   [HttpPost]
   public async Task<IActionResult> Login(LoginDto dto)
   {
      var token = await _authService.LoginAsync(dto.UserName, dto.Password);

      if (token == null)
         return Unauthorized("Fel användarnamn eller lösenord");

      return Ok(new { token });
   }
}