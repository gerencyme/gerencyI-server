using MongoDB.Bson;

namespace Domain.Interfaces.IGeneric
{
    public interface IGenericMongoDb<T> where T : class
    {
        Task Add(T entity);
        Task Delete(T entity);
        Task<T> GetById(ObjectId id);
        Task<List<T>> GetAll();
        Task Update(T entity);
    }
}