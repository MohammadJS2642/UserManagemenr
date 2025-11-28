using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Interfaces;

namespace UserManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest loginRequest)
    {
        var result = await authService.LoginAsync(loginRequest.Username, loginRequest.Password);
        return Ok(result);
    }
}
