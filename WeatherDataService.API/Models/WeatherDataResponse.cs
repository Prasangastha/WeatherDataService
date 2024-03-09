using System.Text.Json.Serialization;
namespace WeatherDataService.API.Models
{
    public class WeatherDataResponse
    {
        [JsonPropertyName("weather")]
        public WeatherData[] Weather { get; set; } = Array.Empty<WeatherData>();
    }

    public class WeatherData
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}