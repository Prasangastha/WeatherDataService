namespace WeatherDataService.API.Configurations
{
    public class WeatherApiKeyOptions
    {
        public int RateLimit { get; set; }
        public List<string> ApiKeys { get; set; }= new List<string>();
    }
}
