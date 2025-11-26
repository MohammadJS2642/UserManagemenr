using UserManagement.Application.Interfaces;

namespace UserManagement.Application.Services;

public interface IAuthorizationService
{
    Task<bool> HasPermissionAsync(int userId, string permissionCode);
}

public class AuthorizationService(
    IUserRepository userRepository,
    IRolePermissionRepository rolePermissionRepository
) : IAuthorizationService
{
    public async Task<bool> HasPermissionAsync(int userId, string permissionCode)
    {
        var currentUser = await userRepository.GetByIdAsync(userId);
        if (currentUser == null || !currentUser.IsActive)
            return false;

        var userRoles = currentUser.Roles.Select(r => r.Id).ToList();

        var hasPermission = await rolePermissionRepository.GetByFilterAsync(r =>
            userRoles.Contains(r.RoleId)
            && r.Permission.Code == permissionCode
        );
        return hasPermission.Any();
    }
}

