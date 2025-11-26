using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;
using UserManagement.Domain.Security;

namespace UserManagement.WebApi.Middleware;

public class PermissionFilter(
    IAuthorizationService authService,
    IUserContextService userContext
) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var permissionAttr = context.ActionDescriptor.EndpointMetadata.OfType<PermissionAttribute>().FirstOrDefault();
        if (permissionAttr == null)
        {
            await next();
            return;
        }

        var userId = userContext.GetCurrentUserId();

        bool allowed = await authService.HasPermissionAsync(userId, permissionAttr.Code);

        if (!allowed)
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }
}
