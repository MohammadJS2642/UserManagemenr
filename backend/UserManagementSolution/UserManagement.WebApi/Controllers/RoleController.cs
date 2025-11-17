using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.UseCases.User;
using UserManagement.WebApi.DTOs;

namespace UserManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController(AssignRoleToUserUseCase assignRoleToUserUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AssignRole(AssignRoleDTO assignRoleDTO)
    {
        await assignRoleToUserUseCase.ExecuteAsync(assignRoleDTO.RoleId, assignRoleDTO.UserId);
        return Ok();
    }
}
