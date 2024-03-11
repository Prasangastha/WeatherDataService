namespace WeatherDataService.API.Interfaces
{
    public interface IApiKeyService
    {
        bool IsValidApiKey(string? apiKey);
        bool IsRateLimited(string? apiKey);
        void IncreaseRequestCount(string? apiKey);
    }
}
