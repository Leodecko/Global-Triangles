using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using WeatherServiceApp.DTO;
using WeatherServiceApp.Model;
using WeatherServiceApp.Services;
using WeatherServiceApp.Services.Interfaces;
using WeatherServiceApp.Services.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace WeatherServiceApp.Controllers
{
    /// <summary>
    /// Weather controller that gets info from https://open-meteo.com/en/doc
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {

        private readonly ILogger<WeatherController> _logger;

        private readonly IWeatherService _weatherService;

        private readonly WeatherRepository _weatherRepository;

        private readonly CityRepository _cityRepository;

        private readonly IMapper _mapper;

        public WeatherController(IMapper mapper,ILogger<WeatherController> logger, IWeatherService weatherService, CityRepository cityRepository, WeatherRepository weatherRepository)
        {
            _logger = logger;

            _mapper = mapper;

            _weatherService = weatherService;

            _weatherRepository = weatherRepository;

            _cityRepository = cityRepository;
        }

        /// <summary>
        /// returns Tempeture, Wind-Direction, Wind-Speed and Sunrise DateTime based on the location.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        [HttpGet("GetWeather")]
        public async Task<IActionResult> Get([Required] double latitude, [Required] double longitude)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var filter = Builders<WeatherModel>.Filter.And(
                    Builders<WeatherModel>.Filter.Eq(u => u.longitude, longitude),
                    Builders<WeatherModel>.Filter.Eq(u => u.latitude, latitude)
                );

                var weatherResult = (await _weatherRepository.ReadAsync(filter)).FirstOrDefault();

                if (weatherResult != null)
                {
                    var mappedResult = _mapper.Map<WeatherGetDto>(weatherResult);

                    return Ok(mappedResult);
                }

                var apiResult = await _weatherService.GetWeather(latitude, longitude);

                await _weatherRepository.CreateAsync(apiResult);

                var result = _mapper.Map<WeatherGetDto>(apiResult);

                return Ok(result);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);

                return BadRequest("Something goes wrong if the error persist please contact It department");


            }
        }

        /// <summary>
        /// returns Tempeture, Wind-Direction, Wind-Speed and Sunrise DateTime based on the location.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        [HttpGet("GetWeatherByCity")]
        public async Task<IActionResult> Get([Required] string cityName)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var filter = Builders<City>.Filter.Eq(u => u.name, cityName);

                var storedCity = (await _cityRepository.ReadAsync(filter)).FirstOrDefault();

                City city = null;

                if (storedCity != null)
                {
                    var weatherFilter = Builders<WeatherModel>.Filter.Eq(u => u.CityId, storedCity.id);

                    var weatherResult = (await _weatherRepository.ReadAsync(weatherFilter)).FirstOrDefault();

                    if(weatherResult!= null)
                    {
                        var mappedResult = _mapper.Map<WeatherGetDto>(weatherResult);

                        return Ok(mappedResult);
                    }

                    //this is in case we have stored the city but no wheater documents are in the collection
                    city = storedCity;
                }
                else
                {
                    // if we have no data stored we get them from the service
                    var cityData = await _weatherService.GetCityData(cityName);

                    city = cityData.results[0];

                    await _cityRepository.CreateAsync(city);
                }

                var apiResult = await _weatherService.GetWeather(city.latitude, city.longitude);
                apiResult.CityId = city.id;


                await _weatherRepository.CreateAsync(apiResult);

                var result = _mapper.Map<WeatherGetDto>(apiResult);

                return Ok(result);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);

                return BadRequest("Something goes wrong if the error persist please contact It department");


            }
        }

    }
}