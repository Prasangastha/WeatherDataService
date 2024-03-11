using System.Net;
using System.Text.Json;
using WeatherDataService.API.Exceptions;

namespace WeatherDataService.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }
        }

        private Task ConvertException(HttpContext context, Exception ex)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var result = string.Empty;

            switch (ex)
            {
                case BadRequestException badRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = badRequestException.Message });
                    break;
                case UnAuthorizedException unAuthorizedException:
                    httpStatusCode = HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(new { error = unAuthorizedException.Message });
                    break;
                case MaximumRateLimitException maximumRateLimitException:
                    httpStatusCode = HttpStatusCode.TooManyRequests;
                    result = JsonSerializer.Serialize(new { error = maximumRateLimitException.Message });
                    break;
                case NotFoundException notFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { error = notFoundException.Message });
                    break;
                case Exception:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    break;
            }

            context.Response.StatusCode = (int)httpStatusCode;

            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = ex.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
