using MongoDB.Driver;
using WeatherServiceApp.Model;

namespace WeatherServiceApp.Services.Repository
{
    public class WeatherRepository : BaseRepository<WeatherModel>
    {
        public WeatherRepository(string databaseName,string collectionName, string connectionString) : base(databaseName,collectionName, connectionString)
        {
        }
    }
}
