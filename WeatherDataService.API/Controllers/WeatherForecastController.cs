using Microsoft.AspNetCore.Mvc;
using System.Net;
using WeatherDataService.API.Interfaces;

namespace WeatherDataService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IWeatherService _weatherService;
        public WeatherForecastController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWeatherAsync(string city = "", string country = "")
        {
            var weather = await _weatherService.GetWeatherAsync(city, country);
            return Ok(weather);
        }
    }
}
