using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;
using System.Net;
using WeatherDataService.API.Configurations;
using WeatherDataService.API.Controllers;
using WeatherDataService.API.Exceptions;
using WeatherDataService.API.Interfaces;
using WeatherDataService.API.Models;
using WeatherDataService.API.Services;
using Microsoft.AspNetCore.Http;
using WeatherDataService.API.Middlewares;

namespace WeatherDataService.Tests
{
    public class WeatherServiceUnitTests
    {
        [Fact]
        public async Task GetWeather_WithValidApiKey_Returns_OkResult()
        {
            // Arrange
            var city = "Melbourne";
            var country = "Australia";
            var expectedWeather = new WeatherForecastDto { Description = "Cloudy" };

            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(service => service.GetWeatherAsync(city, country)).ReturnsAsync(expectedWeather);
            var controller = new WeatherForecastController(weatherServiceMock.Object);

            // Act
            var result = await controller.GetWeatherAsync(city, country);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var weather = Assert.IsAssignableFrom<WeatherForecastDto>(okResult.Value);
            Assert.Equal(expectedWeather.Description, weather.Description);
        }

        [Fact]
        public async Task GetWeatherAsync_InvalidCity_ReturnsNotFound()
        {
            // Arrange
            var city = "InvalidCity";
            var country = "Australia";
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(service => service.GetWeatherAsync(city, country))
                              .ThrowsAsync(new NotFoundException(country, city));
            var controller = new WeatherForecastController(weatherServiceMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await controller.GetWeatherAsync(city, country));

            Assert.Equal($"The description for {country} or {city} is not found. Please enter valid names.", ex.Message);
        }

        [Fact]
        public async Task GetWeatherAsync_EmptyValues_ReturnsBadRequest()
        {
            // Arrange
            var city = "";
            var country = "";
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(service => service.GetWeatherAsync(city, country))
                              .ThrowsAsync(new BadRequestException("Country and City name must be provided."));
            var controller = new WeatherForecastController(weatherServiceMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await controller.GetWeatherAsync(city, country));

            Assert.Equal($"Country and City name must be provided.", ex.Message);
        }

        [Fact]
        public async Task GetWeatherAsync_EmptyCountry_ReturnsBadRequest()
        {
            // Arrange
            var city = "Melbourne";
            var country = "";
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(service => service.GetWeatherAsync(city, country))
                              .ThrowsAsync(new BadRequestException("Country name must be provided."));
            var controller = new WeatherForecastController(weatherServiceMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await controller.GetWeatherAsync(city, country));

            Assert.Equal($"Country name must be provided.", ex.Message);
        }

        [Fact]
        public async Task GetWeatherAsync_EmptyCity_ReturnsBadRequest()
        {
            // Arrange
            var city = "";
            var country = "Australia";
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(service => service.GetWeatherAsync(city, country))
                              .ThrowsAsync(new BadRequestException("City name must be provided."));
            var controller = new WeatherForecastController(weatherServiceMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await controller.GetWeatherAsync(city, country));

            Assert.Equal($"City name must be provided.", ex.Message);
        }

        [Fact]
        public async Task GetWeatherAsync_LongCountryName_ReturnsBadRequest()
        {
            // Arrange
            var city = "Melbourne";
            var country = "AustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustraliaAustralia";
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(service => service.GetWeatherAsync(city, country))
                              .ThrowsAsync(new BadRequestException("Country and City must be less than 90 and 190 characters respectively."));
            var controller = new WeatherForecastController(weatherServiceMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await controller.GetWeatherAsync(city, country));

            Assert.Equal($"Country and City must be less than 90 and 190 characters respectively.", ex.Message);
        }
    }
}
