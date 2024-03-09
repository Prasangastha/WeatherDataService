
using Microsoft.Extensions.Options;
using WeatherDataService.API.Configurations;

namespace WeatherDataService.API.Services
{
    public class WeatherService: IWeatherInterface
    {
        private readonly OpenWeatherMapOptions _options;
        public WeatherService(IOptions<OpenWeatherMapOptions> options)
        {
            _options = options.Value;
        }

        public Task<WeatherForecast> GetWeatherAsync(string city, string country)
        {
            throw new NotImplementedException();
        }
    }
}