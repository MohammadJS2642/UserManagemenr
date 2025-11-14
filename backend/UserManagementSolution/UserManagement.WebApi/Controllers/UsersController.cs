using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.UseCases;

namespace UserManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(CreateUserUseCase createUser) : ControllerBase
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
}
