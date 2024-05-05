using MongoDB.Driver;

namespace WeatherServiceApp.Services.Repository
{
    public class BaseRepository<T>: IGenericRepository<T> where T : class
    {

        private IMongoDatabase _database;
        private IMongoCollection<T> _collection;

        public BaseRepository(string databaseName, string collectionName, string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            _collection = _database.GetCollection<T>(collectionName);
        }

        public async Task CreateAsync(T document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task<List<T>> ReadAsync(FilterDefinition<T> filter)
        {
            var documents = await _collection.Find(filter).ToListAsync();
            return documents;
        }

        public async Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(FilterDefinition<T> filter)
        {
            await _collection.DeleteOneAsync(filter);
        }
    }
}
