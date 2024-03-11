namespace WeatherDataService.API.Exceptions
{
    public class UnAuthorizedException: Exception
    {
        public UnAuthorizedException() : base("API key provided is not valid. Please enter a correct key.")
        {
        }
    }
}
