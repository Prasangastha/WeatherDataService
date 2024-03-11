## Weather Data Service API

This repository contains a simple Web API built with .NET 8 for fetching weather data using the OpenWeatherMap API. The project consists of controllers, services, configurations, and middlewares to handle API requests, caching, rate limiting, and exception handling.

### Project Structure

- **Controllers**: Contains the `WeatherForecastController` responsible for handling weather data retrieval requests.
- **Services**: Contains services like `WeatherService` for fetching weather data from the OpenWeatherMap API and `ApiKeyService` for managing API keys and rate limiting.
- **Configurations**: Contains classes for configuring options like API keys and cache duration.
- **Middlewares**: Contains custom middlewares for handling exceptions (`ExceptionHandlingMiddleware`) and rate limiting (`RateLimitingMiddleware`).

### Code Explanation

- **WeatherForecastController**: Defines a controller with a single endpoint (`GetWeatherAsync`) for fetching weather data based on city and country.
- **ApiKeyService**: Manages API keys and rate limiting. It checks the validity of API keys and ensures that the rate limit is not exceeded.
- **WeatherService**: Fetches weather data from the OpenWeatherMap API. It handles caching of weather data and performs input validation.
- **OpenWeatherMapOptions**: Configuration class for OpenWeatherMap API options like base URL, API keys, and cache duration.
- **WeatherApiKeyOptions**: Configuration class for weather API key options like rate limit and API keys.
- **ExceptionHandlingMiddleware**: Middleware for handling exceptions and returning appropriate HTTP responses with error messages in JSON format.
- **RateLimitingMiddleware**: Middleware for enforcing rate limiting based on API keys.

### How to Use

1. Clone the repository.
2. Configure OpenWeatherMap API and weather API key options in `appsettings.json`.
3. Build and run the application.
4. Access the Swagger UI for testing the API endpoints.
5. Authorize the request in Swagger UI by using any one of the five api keys available in appsettings file.

### User Interface (index.html)

The API can be alternatively tested through the UI. A simple HTML file (`index.html`) is provided in this project to interact with the Weather Data Service API. This UI allows users to input a city and country and fetch weather information using the backend API. The UI utilizes Tailwind CSS for styling and includes JavaScript for making API requests and displaying the weather data.

### Testing

Unit tests for the API can be found in the WeatherDataService.Tests project. These tests ensure the correctness and reliability of the API implementation. Run these tests from Visual Studio to validate the functionality of the API endpoints.


### Note

Ensure that you have valid OpenWeatherMap API keys and configure them appropriately in `appsettings.json` for fetching weather data.

Enjoy testing! 🚀
