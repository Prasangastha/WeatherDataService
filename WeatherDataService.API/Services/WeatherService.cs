
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherDataService.API.Configurations;
using WeatherDataService.API.Exceptions;
using WeatherDataService.API.Interfaces;
using WeatherDataService.API.Models;

namespace WeatherDataService.API.Services
{
    public class WeatherService: IWeatherService
    {
        private readonly HttpClient _httpclient;
        private readonly OpenWeatherMapOptions _options;

        public WeatherService(HttpClient httpClient, IOptions<OpenWeatherMapOptions> options)
        {
            _httpclient = httpClient;
            _options = options.Value;
        }

        public async Task<WeatherForecast> GetWeatherAsync(string city, string country)
        {
            if(string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                throw new BadRequestException("City and country must be provided.");
            }

            var apiKey = _options.ApiKeys.FirstOrDefault();
            var url = $"{_options.BaseUrl}/data/2.5/weather?q={city},{country}&appid={apiKey}";
            var response = await _httpclient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new NotFoundException(country, city);
            }

            var content = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<WeatherDataResponse>(content);

            return new WeatherForecast
            {
                Description = weatherData?.Weather?.FirstOrDefault()?.Description ?? string.Empty
            };

        }
    }
}