using Application.DTO.WeatherForecast;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.WeatherForecasts.Queries;

/// <summary>
/// Query to get a weather forecast by ID
/// </summary>
public class GetWeatherForecastByIdQuery : IRequest<Result<WeatherForecastDto>>
{
    public int Id { get; set; }

    public GetWeatherForecastByIdQuery(int id)
    {
        Id = id;
    }
}

/// <summary>
/// Handler for GetWeatherForecastByIdQuery
/// </summary>
public class GetWeatherForecastByIdQueryHandler : IRequestHandler<GetWeatherForecastByIdQuery, Result<WeatherForecastDto>>
{
    private readonly IWeatherForecastRepository _repository;

    public GetWeatherForecastByIdQueryHandler(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<WeatherForecastDto>> Handle(GetWeatherForecastByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var forecast = await _repository.GetByIdAsync(request.Id);

            if (forecast == null)
                return Result<WeatherForecastDto>.NotFound($"Weather forecast with ID {request.Id} not found");

            var dto = MapToDto(forecast);
            return Result<WeatherForecastDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<WeatherForecastDto>.InternalServerError($"Error retrieving weather forecast: {ex.Message}");
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
