using MongoDB.Bson;
using MongoDB.Driver;

namespace Domain.DomainNewOrderApi.InterfacesNewOrderApi.IGeneric
{
    public interface IGenericMongoDb<T> where T : class
    {
        Task Add(T entity);
        Task Delete(Guid id);
        Task<T> GetById(Guid id);
        Task<List<T>> GetAll();
        Task Update(T entity);
    }
}