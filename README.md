## Weather Data Service API :sunny: :cloud: :umbrella: :zap:

This repository contains a simple Web API built with .NET 8 for fetching weather data using the OpenWeatherMap API. The project consists of controllers, services, configurations, and middlewares to handle API requests, caching, rate limiting, and exception handling.

### Project Structure

- **Controllers**: Contains the `WeatherForecastController` responsible for handling weather data retrieval requests.
- **Services**: Contains services like `WeatherService` for fetching weather data from the OpenWeatherMap API and `ApiKeyService` for managing API keys and rate limiting.
- **Middlewares**: Contains custom middlewares for handling exceptions (`ExceptionHandlingMiddleware`) and rate limiting (`RateLimitingMiddleware`).
- **Configurations**: Contains classes for configuring options like API keys and cache duration.

### Code Explanation

- **WeatherForecastController**: Defines a controller with a single endpoint (`GetWeatherAsync`) for fetching weather data based on city and country.
- **ApiKeyService**: Manages API keys and rate limiting. It checks the validity of API keys and ensures that the hourly rate limit is not exceeded. It uses the in memory cache to validate the rate limit.
- **WeatherService**: Fetches weather data from the OpenWeatherMap API. It handles caching of weather data and performs input validation.
- **RateLimitingMiddleware**: Middleware for enforcing rate limiting based on API keys.
- **ExceptionHandlingMiddleware**: Middleware for handling exceptions and returning appropriate HTTP responses with error messages in JSON format.
- **OpenWeatherMapOptions**: Configuration class for OpenWeatherMap API options like base URL, API keys, and cache duration.
- **WeatherApiKeyOptions**: Configuration class for weather API key options like rate limit and API keys.

### How to Use

1. Clone the repository.
2. Configure OpenWeatherMapAPI in `appsettings.json`. Add the base url and the API Keys. 
3. Build and run the application.
4. Access the Swagger UI for testing the API endpoints.
5. Authorize the request in Swagger UI by using any one of the five api keys available in `appsettings.json` file.

### User Interface (index.html)

The API can be alternatively tested through the UI. A simple HTML file (`index.html`) is provided in this project to interact with the Weather Data Service API. This UI allows users to input a city and country and fetch weather information using the backend API. The UI utilizes Tailwind CSS for styling and includes JavaScript for making API requests and displaying the weather data.

### Testing

Unit tests for the API can be found in the WeatherDataService.Tests project. These tests ensure the correctness and reliability of the API implementation. These tests can be run from Visual Studio to validate the functionality of the API endpoints.


### Notes

- Ensure that you have valid OpenWeatherMap API keys and configure them appropriately in `appsettings.json` for fetching weather data.
- For simplicity (although not recommended), the api keys for the WeatherForecastAPI has already been populated in `appsettings.json` file.
- The response from OpenWeatherMap is cached in memory for 15 minutes according to `CacheMinutes` (can be configured in `appsettings.json`)  for performance.

Enjoy testing! 🚀
