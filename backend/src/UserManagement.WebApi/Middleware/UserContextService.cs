using System.Security.Claims;
using UserManagement.Application.Interfaces;

namespace UserManagement.WebApi.Services;

public class UserContextService(IHttpContextAccessor _httpAccessor) : IUserContextService
{
    public int GetCurrentUserId()
    {
        var userClaim = (_httpAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier))
            ?? throw new UnauthorizedAccessException("User ID not found in token");
        return Convert.ToInt32(userClaim.Value);
    }

    public string? GetCurrentUserName()
    {
        return _httpAccessor.HttpContext?.User?.Identity?.Name;
    }

    public IEnumerable<string?> GetCurrentUserRoles()
    {
        var roleClaims = _httpAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role);
        return roleClaims?.Select(r => r.Value) ?? Enumerable.Empty<string>();
    }
}
