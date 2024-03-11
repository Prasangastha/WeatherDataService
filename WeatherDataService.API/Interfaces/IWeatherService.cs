using WeatherDataService.API.Models;

namespace WeatherDataService.API.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherForecastDto> GetWeatherAsync(string city, string country);
    }
}
