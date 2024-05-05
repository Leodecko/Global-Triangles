using AutoMapper;
using WeatherServiceApp.DTO;
using WeatherServiceApp.Model;

namespace WeatherServiceApp
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() {

            CreateMap<WeatherModel, WeatherGetDto>()
                .ForMember(dest => dest.Wind_Speed, opt => opt.MapFrom(src => src.current.wind_speed_10m))
                .ForMember(dest => dest.Wind_Direction, opt => opt.MapFrom(src => src.current.wind_direction_10m))
                .ForMember(dest => dest.Sunrise, opt => opt.MapFrom(src => src.daily.sunrise))
                .ForMember(dest => dest.Tempeture, opt => opt.MapFrom(src => src.current.temperature_2m));

        }
    }
}
