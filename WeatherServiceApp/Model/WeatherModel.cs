using MongoDB.Bson;

namespace WeatherServiceApp.Model
{
    public class Current
    {
        public string time { get; set; }
        public int interval { get; set; }
        public double temperature_2m { get; set; }
        public double wind_speed_10m { get; set; }
        public double wind_direction_10m { get; set; }
    }

    public class CurrentUnits
    {
        public string time { get; set; }
        public string interval { get; set; }
        public string temperature_2m { get; set; }
        public string wind_speed_10m { get; set; }
        public string wind_direction_10m { get; set; }
    }

    public class Daily
    {
        public List<string> time { get; set; }
        public List<string> sunrise { get; set; }
    }

    public class DailyUnits
    {
        public string time { get; set; }
        public string sunrise { get; set; }
    }

    public class WeatherModel
    {
        public ObjectId Id { get; set; }
        public int CityId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public double elevation { get; set; }
        public CurrentUnits current_units { get; set; }
        public Current current { get; set; }
        public DailyUnits daily_units { get; set; }
        public Daily daily { get; set; }
    }
}
