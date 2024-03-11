using Microsoft.Extensions.Caching.Memory;
using WeatherDataService.API.Services;

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
            if (context.Request.Headers.TryGetValue("X-API-Key", out var apiKeyValues))
            {
                if (apiKeyValues.Count == 0)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("API key is missing");
                    return;
                }
            }

            var apiKey = apiKeyValues.FirstOrDefault();

            if (string.IsNullOrEmpty(apiKey) || !_apiKeyService.IsValidApiKey(apiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API key");
                return; 
            }

            if (_apiKeyService.IsRateLimited(apiKey))
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Hourly rate limit exceeded for API key.");
                return; 
            }

            _apiKeyService.IncreaseRequestCount(apiKey);

            await _next(context);
        }
    }
}
