using WeatherDataService.API.Configurations;
using WeatherDataService.API.Services;
using WeatherDataService.API.Interfaces;
using WeatherDataService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddSingleton<IApiKeyService, ApiKeyService>();

builder.Services.Configure<OpenWeatherMapOptions>(builder.Configuration.GetSection("OpenWeatherMapAPI"));
builder.Services.Configure<WeatherApiKeyOptions>(builder.Configuration.GetSection("WeatherForecastAPI"));

builder.Services.AddMemoryCache();

// Use the custom extension method to add SwaggerGen configuration
builder.Services.AddCustomSwaggerGen(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// For testing purposes, this is not a good practice to allow any origin, method and header
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseExceptionHandlerMiddleware();

app.UseRateLimitMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
