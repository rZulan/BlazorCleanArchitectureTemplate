using Application.Common.Behaviors;
using MediatR;

namespace IMS.Extensions;

/// <summary>
/// Extension methods for configuring the Blazor presentation layer
/// </summary>
public static class PresentationExtensions
{
    /// <summary>
    /// Adds Blazor-specific services and configurations
    /// </summary>
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        // Add MediatR pipeline behaviors for Blazor
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // Add Blazor services
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        return services;
    }
}
