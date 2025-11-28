
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Interfaces;

namespace UserManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController(IPermissionSyncService permissionSyncService) : ControllerBase
{
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("SyncPemissions")]
    public async Task<IActionResult> SyncPemissions()
    {
        var result = await permissionSyncService.SyncPermissionAsync();
        return Ok(new
        {
            Message = "Permissions synchronized successfully.",
            Added = result.Added,
            Removed = result.Removed,
            NewPermission = result.NewPermissions,
            RemovedPermissions = result.RemovedPermissions
        });
    }
}
