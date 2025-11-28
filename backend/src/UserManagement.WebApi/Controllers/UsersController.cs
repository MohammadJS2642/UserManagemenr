using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Interfaces;
using UserManagement.Application.UseCases.User;
using UserManagement.WebApi.Middleware;

namespace UserManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
    GetUsersUseCase getUser,
    CreateUserUseCase createUser,
    DisableUserUseCase _disableUserUseCase,
    IUserRepository _userRepository
) : ControllerBase
{
    [HttpPost]
    //[Permission("user.create", "create user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequests request)
    {
        if (request == null)
        {
            return BadRequest("Request body cannot be null.");
        }
        if (request.Username == null || request.Email == null || request.PasswordHash == null)
        {
            return BadRequest("Username, Email, and Password are required.");
        }

        var result = await createUser.ExecuteAsync(request);
        return CreatedAtAction(nameof(CreateUser), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetUser(int id)
    {
        var userData = await _userRepository.GetByIdAsync(id) ?? throw new Exception("User not found");
        return Ok(userData);
    }

    [HttpGet("GetAllUsers")]
    [Permission("user.getallusers", "Get All Users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await getUser.ExecuteAsync();
        return Ok(users);
    }

    [HttpGet("DisableUser")]
    public async Task<IActionResult> DisableUser(int id)
    {
        var disabled = await _disableUserUseCase.ExecuteAsync(id);
        if (disabled)
            return Ok(new { success = true, mesage = "User disabled successfully." });

        return StatusCode(500, new { success = false, message = "Failed to disable user." });
    }

    [HttpGet("test-auth")]
    [Authorize]
    public IActionResult Test()
    {
        return Ok(new
        {
            Authenticated = User.Identity?.IsAuthenticated,
            Name = User.Identity?.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

}
