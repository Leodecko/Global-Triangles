using MongoDB.Driver;

namespace WeatherServiceApp.Services
{
    public interface IGenericRepository<T> where T : class
    {

        Task CreateAsync(T document);

        Task<List<T>> ReadAsync(FilterDefinition<T> filter);

        Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);

        Task DeleteAsync(FilterDefinition<T> filter);

    }
}
