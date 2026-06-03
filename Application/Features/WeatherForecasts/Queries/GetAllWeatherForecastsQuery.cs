using Application.DTO.WeatherForecast;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.WeatherForecasts.Queries;

/// <summary>
/// Query to get all weather forecasts
/// </summary>
public class GetAllWeatherForecastsQuery : IRequest<Result<List<WeatherForecastDto>>>
{
}

/// <summary>
/// Handler for GetAllWeatherForecastsQuery
/// </summary>
public class GetAllWeatherForecastsQueryHandler : IRequestHandler<GetAllWeatherForecastsQuery, Result<List<WeatherForecastDto>>>
{
    private readonly IWeatherForecastRepository _repository;

    public GetAllWeatherForecastsQueryHandler(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<WeatherForecastDto>>> Handle(GetAllWeatherForecastsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var forecasts = await _repository.GetAllAsync();
            var dtos = forecasts
                .OrderByDescending(f => f.Date)
                .Select(f => MapToDto(f))
                .ToList();

            return Result<List<WeatherForecastDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<WeatherForecastDto>>.InternalServerError($"Error retrieving weather forecasts: {ex.Message}");
        }
    }

    private static WeatherForecastDto MapToDto(WeatherForecast entity)
    {
        return new WeatherForecastDto
        {
            Id = entity.Id,
            Date = entity.Date,
            TemperatureCelsius = entity.TemperatureCelsius,
            TemperatureFahrenheit = entity.TemperatureFahrenheit,
            Summary = entity.Summary,
            Condition = entity.Condition,
            Humidity = entity.Humidity,
            WindSpeed = entity.WindSpeed,
            Location = entity.Location,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsActive = entity.IsActive
        };
    }
}
