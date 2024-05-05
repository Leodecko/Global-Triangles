using WeatherServiceApp.Model;

namespace WeatherServiceApp.Services.Interfaces
{
    public interface IWeatherService
    {
         Task<CityModel> GetCityData(string cityName);

         Task<WeatherModel> GetWeather(string cityName);

        Task<WeatherModel> GetWeather(double latitude, double longitude);
    }
}
