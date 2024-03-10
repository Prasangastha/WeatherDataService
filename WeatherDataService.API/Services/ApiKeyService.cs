using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WeatherDataService.API.Configurations;

namespace WeatherDataService.API.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly List<string> _apiKeys;
        private readonly IMemoryCache _cache;

        public ApiKeyService(IOptions<WeatherApiKeyOptions> options, IMemoryCache cache)
        {
            _apiKeys = options.Value.ApiKeys;
            _cache = cache;
        }

        public bool IsValidApiKey(string? apiKey)
        {
            if(apiKey == null)
            {
                return false;
            }

            return _apiKeys.Contains(apiKey);
        }

        public bool IsRateLimited(string? apiKey)
        {
            var cacheKey = $"RateLimit-{apiKey}-{DateTime.UtcNow:yyyy-MM-dd-HH}";

            if (!_cache.TryGetValue(cacheKey, out int requestCount))
            {
                requestCount = 0;
            }

            return requestCount >= 5;
        }
        public void IncreaseRequestCount(string? apiKey)
        {
            var cacheKey = $"RateLimit-{apiKey}-{DateTime.UtcNow:yyyy-MM-dd-HH}";

            if (!_cache.TryGetValue(cacheKey, out int requestCount))
            {
                requestCount = 0;
            }

            requestCount++;
            _cache.Set(cacheKey, requestCount, TimeSpan.FromHours(1));
        }
 
    }
}
