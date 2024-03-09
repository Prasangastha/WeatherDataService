using Microsoft.AspNetCore.Mvc;
using WeatherDataService.API.Services;

namespace WeatherDataService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherAsync(string city, string country)
        {
            var weatherService = new WeatherService();
            var weather = await weatherService.GetWeatherAsync(city, country);
            return Ok(weather);
        }
    }
}
