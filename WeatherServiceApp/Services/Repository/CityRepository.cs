using WeatherServiceApp.Model;

namespace WeatherServiceApp.Services.Repository
{
    public class CityRepository : BaseRepository<City>
    {
        public CityRepository(string databaseName, string collectionName, string connectionString) : base(databaseName, collectionName, connectionString)
        {
        }
    }
}
