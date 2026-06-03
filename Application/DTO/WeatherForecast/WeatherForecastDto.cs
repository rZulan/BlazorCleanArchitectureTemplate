namespace Application.DTO.WeatherForecast;

/// <summary>
/// Data transfer object for WeatherForecast
/// </summary>
public class WeatherForecastDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public int TemperatureFahrenheit { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public double Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
