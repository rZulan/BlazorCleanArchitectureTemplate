using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Repository interface for WeatherForecast entity
/// </summary>
public interface IWeatherForecastRepository
{
    Task<WeatherForecast?> GetByIdAsync(int id);
    Task<IEnumerable<WeatherForecast>> GetAllAsync();
    Task AddAsync(WeatherForecast entity);
    Task UpdateAsync(WeatherForecast entity);
    Task DeleteAsync(WeatherForecast entity);
    Task SaveChangesAsync();
}
