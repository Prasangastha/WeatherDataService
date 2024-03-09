namespace WeatherDataService.API.Services
{
    public interface IWeatherInterface
    {
        Task<WeatherForecast> GetWeatherAsync(string city, string country);
    }
}
