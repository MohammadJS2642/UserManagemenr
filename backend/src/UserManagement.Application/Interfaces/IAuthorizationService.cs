namespace UserManagement.Application.Interfaces;

public interface IAuthorizationService
{
    Task<bool> HasPermissionAsync(int userId, string permissionCode);
}
