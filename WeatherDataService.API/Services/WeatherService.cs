
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(HttpClient httpClient, IOptions<OpenWeatherMapOptions> options, IMemoryCache cache, ILogger<WeatherService> logger)
        {
            _httpclient = httpClient;
            _options = options.Value;
            _cache = cache;
            _logger = logger;
        }

        public async Task<WeatherForecast> GetWeatherAsync(string city, string country)
        {
            ValidateInput(city, country);

            WeatherForecast weatherForecast = GetWeatherForecastFromCache(city, country);

            if (!string.IsNullOrEmpty(weatherForecast.Description))
            {
                return weatherForecast;
            }

            string? apiKey = _options.ApiKeys.FirstOrDefault();
            string url = $"{_options.BaseUrl}/data/2.5/weather?q={city},{country}&appid={apiKey}";
            _logger.LogInformation($"Fetching weather data for {city}, {country}");

            try
            {
                HttpResponseMessage response = await _httpclient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                WeatherDataResponse? weatherData = JsonSerializer.Deserialize<WeatherDataResponse>(content);
                string weatherDescription = weatherData?.Weather?.FirstOrDefault()?.Description ?? string.Empty;

                if (string.IsNullOrEmpty(weatherDescription))
                {
                    _logger.LogError($"Weather data for {city}, {country} is empty");
                    throw new NotFoundException(country, city);
                }

                SetWeatherForecastInCache(city, country, weatherDescription);

                weatherForecast.Description = weatherDescription;

                return weatherForecast;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching weather data for {city}, {country} : {ex.Message}");
                throw new BadRequestException("Failed to fetch the description for the weather from OpenWeatherMap");
            }

        }

        public WeatherForecast GetWeatherForecastFromCache(string city, string country)
        {
            if (_cache.TryGetValue($"{city}-{country}", out WeatherForecast? weatherForecast))
            {
                _logger.LogInformation($"Fetching weather data for {city}, {country} from cache");
                return weatherForecast ?? new WeatherForecast();
            }

            return new WeatherForecast();
        }

        public void SetWeatherForecastInCache(string city, string country, string description)
        {
            _logger.LogInformation($"Setting weather data for {city}, {country} in cache");
            _cache.Set($"{city}-{country}", new WeatherForecast
            {
                Description = description
            }, TimeSpan.FromMinutes(_options.CacheDurationMinutes));
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