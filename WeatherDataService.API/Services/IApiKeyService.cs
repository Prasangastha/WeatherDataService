namespace WeatherDataService.API.Services
{
    public interface IApiKeyService
    {
        bool IsValidApiKey(string apiKey);
        bool IsRateLimited(string apiKey);
        void IncreaseRequestCount(string apiKey);
    }
}
