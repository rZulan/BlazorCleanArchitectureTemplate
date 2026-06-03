using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

/// <summary>
/// Dependency injection extensions for the Application layer
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });

        return services;
    }
}
