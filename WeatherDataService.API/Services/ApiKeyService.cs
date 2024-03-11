using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WeatherDataService.API.Configurations;
using WeatherDataService.API.Exceptions;
using WeatherDataService.API.Interfaces;

namespace WeatherDataService.API.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly List<string> _apiKeys;
        private readonly int _rateLimit;
        private readonly IMemoryCache _cache;

        public ApiKeyService(IOptions<WeatherApiKeyOptions> options, IMemoryCache cache)
        {
            _apiKeys = options.Value.ApiKeys;
            _rateLimit = options.Value.RateLimit;
            _cache = cache;
        }

        public bool IsValidApiKey(string? apiKey)
        {
            if(apiKey == null)
            {
                throw new BadRequestException("API key is missing");
            }

            return _apiKeys.Contains(apiKey);
        }

        public bool IsRateLimited(string? apiKey)
        {
            var cacheKey = GetCacheKey();

            if (!_cache.TryGetValue(cacheKey, out int requestCount))
            {
                requestCount = 0;
            }

            return requestCount >= _rateLimit;
        }
        public void IncreaseRequestCount(string? apiKey)
        {
            var cacheKey = GetCacheKey();

            if (!_cache.TryGetValue(cacheKey, out int requestCount))
            {
                requestCount = 0;
            }

            requestCount++;
            _cache.Set(cacheKey, requestCount, TimeSpan.FromHours(1));
        }

        private static string GetCacheKey() => $"RateLimit-{DateTime.UtcNow:yyyy-MM-dd-HH}";
 
    }
}
