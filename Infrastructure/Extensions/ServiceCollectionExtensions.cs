using Infrastructure.Data;
using Infrastructure.Repositories;
using Application.Interfaces;
using Domain.Entities;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

/// <summary>
/// Dependency injection extensions for the Infrastructure layer
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, sqlServerOptions =>
                sqlServerOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Register DbContext as base class for dependency injection
        services.AddScoped(provider => provider.GetRequiredService<ApplicationDbContext>() as DbContext);

        // Register specific repositories
        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}

