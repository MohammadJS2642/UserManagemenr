namespace UserManagement.Application.Interfaces;

public interface IUserContextService
{
    int GetCurrentUserId();
    string? GetCurrentUserName();
    IEnumerable<string?> GetCurrentUserRoles();
}
