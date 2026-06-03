using Application.DTO.WeatherForecast;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.WeatherForecasts.Commands;

/// <summary>
/// Command to update a weather forecast
/// </summary>
public class UpdateWeatherForecastCommand : IRequest<Result<WeatherForecastDto>>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public double Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Handler for UpdateWeatherForecastCommand
/// </summary>
public class UpdateWeatherForecastCommandHandler : IRequestHandler<UpdateWeatherForecastCommand, Result<WeatherForecastDto>>
{
    private readonly IWeatherForecastRepository _repository;

    public UpdateWeatherForecastCommandHandler(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<WeatherForecastDto>> Handle(UpdateWeatherForecastCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Summary))
                return Result<WeatherForecastDto>.BadRequest("Summary is required");

            if (string.IsNullOrWhiteSpace(request.Location))
                return Result<WeatherForecastDto>.BadRequest("Location is required");

            if (request.Date == default)
                return Result<WeatherForecastDto>.BadRequest("Date is required");

            // Get existing forecast
            var forecast = await _repository.GetByIdAsync(request.Id);
            if (forecast == null)
                return Result<WeatherForecastDto>.NotFound($"Weather forecast with ID {request.Id} not found");

            // Update properties
            forecast.Date = request.Date;
            forecast.TemperatureCelsius = request.TemperatureCelsius;
            forecast.Summary = request.Summary.Trim();
            forecast.Condition = request.Condition.Trim();
            forecast.Humidity = request.Humidity;
            forecast.WindSpeed = request.WindSpeed;
            forecast.Location = request.Location.Trim();

            // Save
            await _repository.UpdateAsync(forecast);
            await _repository.SaveChangesAsync();

            var dto = MapToDto(forecast);
            return Result<WeatherForecastDto>.Success(dto, "Weather forecast updated successfully");
        }
        catch (Exception ex)
        {
            return Result<WeatherForecastDto>.InternalServerError($"Error updating weather forecast: {ex.Message}");
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
