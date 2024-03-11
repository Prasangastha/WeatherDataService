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

        public RateLimitingMiddleware(RequestDelegate next, IApiKeyService apiKeyService)
        {
            _next = next;
            _apiKeyService = apiKeyService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-API-Key", out StringValues apiKeyValues))
            {
                if (apiKeyValues.Count == 0)
                {
                    throw new BadRequestException("API key is missing");
                }
            }

            string? apiKey = apiKeyValues.FirstOrDefault();

            if (!_apiKeyService.IsValidApiKey(apiKey))
            {
                throw new UnAuthorizedException();
            }

            if (_apiKeyService.IsRateLimited(apiKey))
            {
                throw new MaximumRateLimitException();
            }

            _apiKeyService.IncreaseRequestCount(apiKey);

            await _next(context);
        }
    }
}
