using Microsoft.AspNetCore.Mvc;
using WeatherDataService.API.Interfaces;

namespace WeatherDataService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IWeatherService weatherService, ILogger<WeatherForecastController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherAsync(string city, string country)
        {
            var weather = await _weatherService.GetWeatherAsync(city, country);
            return Ok(weather);
        }
    }
}
