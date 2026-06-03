using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a weather forecast
/// </summary>
public class WeatherForecast : BaseEntity
{
    public DateTime Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public int TemperatureFahrenheit => (TemperatureCelsius * 9 / 5) + 32;
    public string Summary { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public double Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string Location { get; set; } = string.Empty;
}
