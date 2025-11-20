using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Interfaces;
using UserManagement.Application.UseCases.User;

namespace UserManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
    CreateUserUseCase createUser, 
    DisableUserUseCase _disableUserUseCase,
    IUserRepository _userRepository
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] DTOs.CreateUserRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request body cannot be null.");
        }
        if (request.Username == null || request.Email == null || request.Password == null)
        {
            return BadRequest("Username, Email, and Password are required.");
        }

        var result = await createUser.ExecuteAsync(request.Username, request.Email, request.Password);
        return CreatedAtAction(nameof(CreateUser), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetUser(int id)
    {
        var userData = await _userRepository.GetByIdAsync(id) ?? throw new Exception("User not found");
        return Ok(userData);
    }

    [HttpGet("/GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("DisableUser")]
    public async Task<IActionResult> DisableUser(int id)
    {
        var disabled = await _disableUserUseCase.ExecuteAsync(id);
        switch (disabled)
        {
            case true:
                return Ok("User disabled successfully.");
            case false:
                return StatusCode(500, "Failed to disable user.");
        }
    }
}
