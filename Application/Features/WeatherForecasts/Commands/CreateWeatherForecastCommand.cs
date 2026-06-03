using Application.DTO.WeatherForecast;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.WeatherForecasts.Commands;

/// <summary>
/// Command to create a new weather forecast
/// </summary>
public class CreateWeatherForecastCommand : IRequest<Result<WeatherForecastDto>>
{
    public DateTime Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public double Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Handler for CreateWeatherForecastCommand
/// </summary>
public class CreateWeatherForecastCommandHandler : IRequestHandler<CreateWeatherForecastCommand, Result<WeatherForecastDto>>
{
    private readonly IWeatherForecastRepository _repository;

    public CreateWeatherForecastCommandHandler(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<WeatherForecastDto>> Handle(CreateWeatherForecastCommand request, CancellationToken cancellationToken)
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

            // Create entity
            var forecast = new WeatherForecast
            {
                Date = request.Date,
                TemperatureCelsius = request.TemperatureCelsius,
                Summary = request.Summary.Trim(),
                Condition = request.Condition.Trim(),
                Humidity = request.Humidity,
                WindSpeed = request.WindSpeed,
                Location = request.Location.Trim()
            };

            // Save
            await _repository.AddAsync(forecast);
            await _repository.SaveChangesAsync();

            var dto = MapToDto(forecast);
            return Result<WeatherForecastDto>.Success(dto, "Weather forecast created successfully");
        }
        catch (Exception ex)
        {
            return Result<WeatherForecastDto>.InternalServerError($"Error creating weather forecast: {ex.Message}");
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
