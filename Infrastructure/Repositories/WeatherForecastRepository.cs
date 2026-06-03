using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Repository implementation for WeatherForecast entity
/// </summary>
public class WeatherForecastRepository : IWeatherForecastRepository
{
    protected readonly DbContext _context;

    public WeatherForecastRepository(DbContext context)
    {
        _context = context;
    }

    public virtual async Task<WeatherForecast?> GetByIdAsync(int id)
    {
        return await _context.Set<WeatherForecast>().FindAsync(id);
    }

    public virtual async Task<IEnumerable<WeatherForecast>> GetAllAsync()
    {
        return await _context.Set<WeatherForecast>().AsNoTracking().ToListAsync();
    }

    public virtual async Task AddAsync(WeatherForecast entity)
    {
        await _context.Set<WeatherForecast>().AddAsync(entity);
    }

    public virtual async Task UpdateAsync(WeatherForecast entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Set<WeatherForecast>().Update(entity);
    }

    public virtual async Task DeleteAsync(WeatherForecast entity)
    {
        _context.Set<WeatherForecast>().Remove(entity);
    }

    public virtual async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
