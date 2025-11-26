using Microsoft.Extensions.DependencyInjection;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;

namespace UserManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection ApplicationLayerInjection(this IServiceCollection services)
    {
        // Register application services here
        services.AddAutoMapper(m => m.AddProfile<Mapping.UserProfile>());

        services.AddScoped<IAuthorizationService, AuthorizationService>();
        return services;
    }
}
