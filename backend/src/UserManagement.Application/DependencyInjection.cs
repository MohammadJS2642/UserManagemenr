using Microsoft.Extensions.DependencyInjection;

namespace UserManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection ApplicationLayerInjection(this IServiceCollection services)
    {
        // Register application services here
        services.AddAutoMapper(m => m.AddProfile<Mapping.UserProfile>());
        return services;
    }
}
