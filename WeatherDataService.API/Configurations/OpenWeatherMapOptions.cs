namespace WeatherDataService.API.Configurations
{
    public class OpenWeatherMapOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public List<string> ApiKeys { get; set; } = new List<string>();
        public int CacheDurationMinutes { get; set; }
    }
}