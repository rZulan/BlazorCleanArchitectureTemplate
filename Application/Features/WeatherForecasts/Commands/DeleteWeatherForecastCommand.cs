using Application.Common.Results;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.WeatherForecasts.Commands;

/// <summary>
/// Command to delete a weather forecast
/// </summary>
public class DeleteWeatherForecastCommand : IRequest<Result>
{
    public int Id { get; set; }

    public DeleteWeatherForecastCommand(int id)
    {
        Id = id;
    }
}

/// <summary>
/// Handler for DeleteWeatherForecastCommand
/// </summary>
public class DeleteWeatherForecastCommandHandler : IRequestHandler<DeleteWeatherForecastCommand, Result>
{
    private readonly IWeatherForecastRepository _repository;

    public DeleteWeatherForecastCommandHandler(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteWeatherForecastCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing forecast
            var forecast = await _repository.GetByIdAsync(request.Id);
            if (forecast == null)
                return Result.NotFound($"Weather forecast with ID {request.Id} not found");

            // Delete
            await _repository.DeleteAsync(forecast);
            await _repository.SaveChangesAsync();

            return Result.Success("Weather forecast deleted successfully");
        }
        catch (Exception ex)
        {
            return Result.InternalServerError($"Error deleting weather forecast: {ex.Message}");
        }
    }
}
