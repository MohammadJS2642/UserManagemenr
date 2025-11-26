using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.UseCases.RoleUseCase;
using UserManagement.Domain.Security;

namespace UserManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController(
    GetAllRolesUseCase getAllRoles,
    GetRoleUseCase getRole,
    CreateRoleUseCase createRole,
    DeleteRoleUseCase deleteRole
) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleRequests requests)
    {
        var roleId = await createRole.ExecuteAsync(requests);
        return Ok(roleId);
    }

    [HttpGet("GetRoles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await getAllRoles.ExecuteAsync();
        return Ok(roles);
    }

    [HttpGet("GetRole")]
    public async Task<IActionResult> GetRole(int id)
    {
        var role = await getRole.ExecuteAsync(id);
        return Ok(role);
    }

    [HttpGet("DeleteRole")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        await deleteRole.ExecuteAsync(id);
        return NoContent();
    }

    //[HttpPost]
    //public async Task<IActionResult> AssignRole(AssignRoleDTO assignRoleDTO)
    //{
    //    await assignRoleToUserUseCase.ExecuteAsync(assignRoleDTO.RoleId, assignRoleDTO.UserId);
    //    return Ok();
    //}
}
