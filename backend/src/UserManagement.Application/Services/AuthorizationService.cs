using UserManagement.Application.Interfaces;

namespace UserManagement.Application.Services;

public class AuthorizationService(
    IUserRepository userRepository,
    IRolePermissionRepository rolePermissionRepository
) : IAuthorizationService
{
    public async Task<bool> HasPermissionAsync(int userId, string permissionCode)
    {
        var currentUser = await userRepository.GetUserByRoles(u => u.Id == userId);
        if (currentUser == null || !currentUser.IsActive)
            return false;

        var userRoles = currentUser.UserRoles.Select(r => r.RoleId).ToList();

        var hasPermission = await rolePermissionRepository.GetByFilterAsync(r =>
            userRoles.Contains(r.RoleId)
            && r.Permission.Code == permissionCode
        );
        return hasPermission.Any();
    }
}

