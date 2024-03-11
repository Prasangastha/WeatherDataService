
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
            
            ValidateInput(city, country);   

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

        public void ValidateInput(string city, string country)
        {
            if(string.IsNullOrEmpty(country) && string.IsNullOrEmpty(city))
            {
                throw new BadRequestException("Country and City name must be provided.");
            }

            if(string.IsNullOrEmpty(country))
            {
                throw new BadRequestException("Country name must be provided.");
            }

            if(string.IsNullOrEmpty(city))
            {
                throw new BadRequestException("City name must be provided.");
            }

            if(country.Length > 90 || city.Length > 190)
            {
                throw new BadRequestException("Country and City must be less than 90 and 190 characters respectively.");
            }
        }
    }
}