namespace WeatherDataService.API.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException(string country, string city) 
            : base($"The description for {country} or {city} is not found. Please enter valid names.")
        {
        }
    }
}
