using Microsoft.Extensions.Configuration;
using MongoDB.Embedded;
using WeatherServiceApp.Services;
using WeatherServiceApp.Services.Interfaces;
using WeatherServiceApp.Services.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IWeatherService, WeatherService>();

string connectionString = builder.Configuration.GetConnectionString("MongoDB");

builder.Services.AddScoped<WeatherRepository>(sp => new WeatherRepository("defaultApp","Weather",connectionString));

builder.Services.AddScoped<CityRepository>(sp => new CityRepository("defaultApp", "City", connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
