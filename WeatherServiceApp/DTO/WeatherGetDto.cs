namespace WeatherServiceApp.DTO
{
    public class WeatherGetDto
    {
        public decimal Tempeture { get; set; }

        public int Wind_Direction { get; set; }

        public int Wind_Speed { get; set; }

        public List<string> Sunrise { get; set; }
    }
    

}
