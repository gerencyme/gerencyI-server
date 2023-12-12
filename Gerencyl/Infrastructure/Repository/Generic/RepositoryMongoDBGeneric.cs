using Domain.Interfaces.IGeneric;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repository.Generic
{
    public class RepositoryMongoDBGeneric<T> : IGenericMongoDb<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public RepositoryMongoDBGeneric(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task Add(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Delete(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", GetIdValue(entity));
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<T> GetById(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task Update(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", GetIdValue(entity));
            await _collection.ReplaceOneAsync(filter, entity);
        }

        private int GetIdValue(T entity)
        {
            var propertyInfo = typeof(T).GetProperty("Id"); // Assuming your MongoDB entities have an "Id" property.
            if (propertyInfo == null)
            {
                throw new InvalidOperationException("Entity does not have an 'Id' property.");
            }

            return (int)propertyInfo.GetValue(entity);
        }

    }
}