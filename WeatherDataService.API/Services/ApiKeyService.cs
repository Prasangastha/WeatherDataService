using Microsoft.Extensions.Options;
using WeatherDataService.API.Configurations;

namespace WeatherDataService.API.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly List<string> _apiKeys;

        public ApiKeyService(IOptions<WeatherApiKeyOptions> options)
        {
            _apiKeys = options.Value.ApiKeys;
        }
        public void IncreaseRequestCount(string apiKey)
        {
            throw new NotImplementedException();
        }

        public bool IsRateLimited(string apiKey)
        {
            throw new NotImplementedException();
        }

        public bool IsValidApiKey(string apiKey)
        {
            throw new NotImplementedException();
        }
    }
}
