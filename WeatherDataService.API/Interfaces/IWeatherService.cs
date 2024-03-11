using WeatherDataService.API.Models;

namespace WeatherDataService.API.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetWeatherAsync(string city, string country);
    }
}
