using WeatherDataService.API.Models;

namespace WeatherDataService.API.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetWeatherAsync(string city, string country);
    }
}
