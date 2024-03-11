using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using WeatherDataService.API.Configurations;
using WeatherDataService.API.Exceptions;
using WeatherDataService.API.Interfaces;
using WeatherDataService.API.Middlewares;
using WeatherDataService.API.Services;

namespace WeatherDataService.Tests
{
    public class ApiKeyServiceUnitTests
    {

        [Fact]
        public void IsValidApiKey_ValidKey_ReturnsTrue()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<WeatherApiKeyOptions>>();
            optionsMock.SetupGet(o => o.Value).Returns(new WeatherApiKeyOptions { ApiKeys = new List<string> { "valid_key" }, RateLimit = 5 });
            var cacheMock = new Mock<IMemoryCache>();
            var service = new ApiKeyService(optionsMock.Object, cacheMock.Object);

            // Act
            var result = service.IsValidApiKey("valid_key");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidApiKey_InvalidKey_ReturnsFalse()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<WeatherApiKeyOptions>>();
            optionsMock.SetupGet(o => o.Value).Returns(new WeatherApiKeyOptions { ApiKeys = new List<string> { "valid_key" }, RateLimit = 5 });
            var cacheMock = new Mock<IMemoryCache>();
            var service = new ApiKeyService(optionsMock.Object, cacheMock.Object);

            // Act
            var result = service.IsValidApiKey("invalid_key");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsRateLimited_RateLimitExceeded_ReturnsFalse()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<WeatherApiKeyOptions>>();
            optionsMock.SetupGet(o => o.Value).Returns(new WeatherApiKeyOptions { ApiKeys = new List<string> { "valid_key" }, RateLimit = 2 });
            var cacheMock = new Mock<IMemoryCache>();

            var service = new ApiKeyService(optionsMock.Object, cacheMock.Object);

            // Act
            var result = service.IsRateLimited("valid_key");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task InvokeAsync_InvalidKey_ThrowsUnAuthorizedException()
        {
            // Arrange
            var next = new RequestDelegate(context => Task.CompletedTask);
            var apiKeyServiceMock = new Mock<IApiKeyService>();
            apiKeyServiceMock.Setup(s => s.IsValidApiKey("valid_key")).Returns(false);

            var loggerMock = new Mock<ILogger<RateLimitingMiddleware>>();
            var middleware = new RateLimitingMiddleware(next, apiKeyServiceMock.Object, loggerMock.Object);
            var context = new DefaultHttpContext();
            context.Request.Headers.Append("X-API-Key", "valid_key");

            // Act & Assert
            await Assert.ThrowsAsync<UnAuthorizedException>(() => middleware.InvokeAsync(context));
        }

        [Fact]
        public async Task InvokeAsync_RateLimitExceeded_ThrowsMaximumRateLimitException()
        {
            // Arrange
            var next = new RequestDelegate(context => Task.CompletedTask);
            var apiKeyServiceMock = new Mock<IApiKeyService>();
            apiKeyServiceMock.Setup(s => s.IsValidApiKey("valid_key")).Returns(true);
            apiKeyServiceMock.Setup(s => s.IsRateLimited("valid_key")).Returns(true);
            var loggerMock = new Mock<ILogger<RateLimitingMiddleware>>();
            var middleware = new RateLimitingMiddleware(next, apiKeyServiceMock.Object, loggerMock.Object);
            var context = new DefaultHttpContext();
            context.Request.Headers.Append("X-API-Key", "valid_key");

            // Act & Assert
            await Assert.ThrowsAsync<MaximumRateLimitException>(() => middleware.InvokeAsync(context));
        }

        [Fact]
        public async Task Invoke_ExceptionOccurs_HandlesException()
        {
            // Arrange
            var next = new RequestDelegate(context => throw new Exception("Test exception"));
            var middleware = new ExceptionHandlingMiddleware(next);
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.Invoke(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains("Test exception", responseBody);
            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        }
    }
}
