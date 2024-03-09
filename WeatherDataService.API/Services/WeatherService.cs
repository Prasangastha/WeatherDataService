
namespace WeatherDataService.API.Services
{
    public class WeatherService: IWeatherInterface
    {
        public WeatherService()
        {
        }

        public Task<WeatherForecast> GetWeatherAsync(string city, string country)
        {
            throw new NotImplementedException();
        }
    }
}