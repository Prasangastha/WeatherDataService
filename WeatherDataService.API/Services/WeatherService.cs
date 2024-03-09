
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherDataService.API.Configurations;
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
            var apiKey = _options.ApiKeys.FirstOrDefault();
            var url = $"{_options.BaseUrl}/data/2.5/weather?q={city},{country}&appid={apiKey}";
            var response = await _httpclient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch weather data from OpenWeatherMap.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<WeatherDataResponse>(content);

            return new WeatherForecast
            {
                description = weatherData?.weather?.FirstOrDefault()?.description ?? string.Empty
            };

        }
    }
}