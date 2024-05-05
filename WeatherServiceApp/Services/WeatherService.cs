using System.Text.Json;
using System.Text.Json.Serialization;
using WeatherServiceApp.Model;
using WeatherServiceApp.Services.Interfaces;

namespace WeatherServiceApp.Services
{
    public class WeatherService : IWeatherService
    {
        private string _urlWeatherApi;
        private string _geoLocationApi;
        public WeatherService(IConfiguration configuration)
        {
            _urlWeatherApi = configuration["WeatherEndpoint"];

            _geoLocationApi = configuration["GeoLocationEndpoint"];
        }

        /// <summary>
        /// returns weather info by latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<WeatherModel> GetWeather(double latitude, double longitude)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpointUrl = $"{_urlWeatherApi}/v1/forecast?latitude={latitude}&longitude={longitude}&daily=sunrise&current=temperature_2m,wind_speed_10m,wind_direction_10m,wind_direction_10m";

                HttpResponseMessage response = await client.GetAsync(endpointUrl);

                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<WeatherModel>(content);
                }

                throw new Exception($"WeatherService -> GetWeather Latitude longitude-> Endpoint throws an unexpected result {response.StatusCode} -> {content}");
            }
        }

        /// <summary>
        /// return Weather info by City
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<WeatherModel> GetWeather(string cityName)
        {
            CityModel CityResult = await GetCityData(cityName);

            bool hasCity = CityResult.results.Count > 0;

            if(!hasCity)
                throw new Exception($"WeatherService -> GetWeather City -> City {cityName} -> was not found");

            City city = CityResult.results.FirstOrDefault();

            return await GetWeather(city.latitude, city.longitude);
        }


        /// <summary>
        /// Returns Geo Info as latitude and longitude by City Name
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CityModel> GetCityData(string cityName)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpointUrl = $"{_geoLocationApi}v1/search?name={cityName}";

                HttpResponseMessage response = await client.GetAsync(endpointUrl);

                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<CityModel>(content);
                }

                throw new Exception($"WeatherService -> GetCityData -> Endpoint throws an unexpected result {response.StatusCode} -> {content}");
            }
        }

    }
}
