using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;

namespace UserManagement.WebApi.Middleware;

public class PermissionFilter(
    IAuthorizationService authService,
    IUserContextService userContext
) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //var permissionAttr = context.ActionDescriptor.EndpointMetadata.OfType<PermissionAttribute>().FirstOrDefault();
        var controllerAction = context.ActionDescriptor as ControllerActionDescriptor;
        if (controllerAction == null)
        {
            await next();
            return;
        }

        var permissionAttr = controllerAction.MethodInfo
            .GetCustomAttributes(typeof(PermissionAttribute), inherit: true)
            .FirstOrDefault() as PermissionAttribute;

        if (permissionAttr == null)
        {
            await next();
            return;
        }

        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            context.Result = new UnauthorizedResult();
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

    //public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    //{
    //    if (!context.HttpContext.User.Identity?.IsAuthenticated ?? false)
    //    {
    //        context.Result = new UnauthorizedResult();
    //        return;
    //    }

    //    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
    //    var attr = descriptor.MethodInfo.GetCustomAttribute<PermissionAttribute>();

    //    if (attr == null)
    //        return;

    //    var userId = userContext.GetCurrentUserId();

    //    var has = await authService.HasPermissionAsync(userId, attr.Code);
    //    if (!has)
    //        context.Result = new ForbidResult();
    //}
}
