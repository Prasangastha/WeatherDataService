using Microsoft.Extensions.Configuration;
using WeatherDataService.API.Configurations;
using WeatherDataService.API.Middlewares;
using WeatherDataService.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddSingleton<IApiKeyService, ApiKeyService>();
builder.Services.Configure<OpenWeatherMapOptions>(builder.Configuration.GetSection("OpenWeatherMap"));
builder.Services.Configure<WeatherApiKeyOptions>(builder.Configuration.GetSection("WeatherForecastAPI"));
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimitMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
