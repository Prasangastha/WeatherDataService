using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using WeatherDataService.API.Exceptions;
using WeatherDataService.API.Interfaces;

namespace WeatherDataService.API.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiKeyService _apiKeyService;
        private readonly ILogger<RateLimitingMiddleware> _logger;

        public RateLimitingMiddleware(RequestDelegate next, IApiKeyService apiKeyService, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _apiKeyService = apiKeyService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("x-api-key", out StringValues apiKeyValues))
            {
                if (apiKeyValues.Count == 0)
                {
                    _logger.LogError("API key is missing");
                    throw new BadRequestException("API key is missing");
                }
            }

            string? apiKey = apiKeyValues.FirstOrDefault();

            if (!_apiKeyService.IsValidApiKey(apiKey))
            {
                _logger.LogError("Invalid API key");
                throw new UnAuthorizedException();
            }

            if (_apiKeyService.IsRateLimited(apiKey))
            {
                _logger.LogError("Rate limit exceeded");
                throw new MaximumRateLimitException();
            }

            _apiKeyService.IncreaseRequestCount(apiKey);

            await _next(context);
        }
    }
}
