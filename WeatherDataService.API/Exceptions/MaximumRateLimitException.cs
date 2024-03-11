namespace WeatherDataService.API.Exceptions
{
    public class MaximumRateLimitException: Exception
    {
        public MaximumRateLimitException() : base("Maximum rate limit reached. Please try again later.")
        {
        }
    }
}
