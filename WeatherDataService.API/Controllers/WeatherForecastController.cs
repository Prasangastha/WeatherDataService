using Microsoft.AspNetCore.Mvc;
using WeatherDataService.API.Interfaces;
using WeatherDataService.API.Models;

namespace WeatherDataService.API.Controllers
{

    [ApiController]
    [Route("/weather")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IWeatherService _weatherService;
        public WeatherForecastController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(WeatherForecastDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetWeatherAsync(string city = "", string country = "")
        {
            var weather = await _weatherService.GetWeatherAsync(city, country);
            return Ok(weather);
        }
    }
}
