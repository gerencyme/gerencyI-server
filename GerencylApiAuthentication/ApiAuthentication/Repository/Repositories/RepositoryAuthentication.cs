using ApiAuthentication.Models;
using ApiAuthentication.Services.Interfaces.InterfacesRepositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Repository.Generic;

namespace ApiAuthentication.Repository
{
    public class RepositoryAuthentication : RepositoryMongoDBGeneric<GerencylRegister>, IAuthenticationRepository
    {
        private readonly IMongoCollection<GerencylRegister> _collection;

        public RepositoryAuthentication(IOptions<MongoDbSettings> settings) : base(settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<GerencylRegister>("Users");
        }
        public async Task UpdateNewOrder(GerencylRegister entity, string id)
        {
            var filter = Builders<GerencylRegister>.Filter.Eq("CNPJ", id);
            var existingEntity = await _collection.Find(filter).FirstOrDefaultAsync();

            if (existingEntity != null)
            {
                entity.Id = existingEntity.Id;
                entity.Name = existingEntity.Name;
                entity.CNPJ = existingEntity.CNPJ;
                entity.Claims = existingEntity.Claims;
                entity.CreationDate = existingEntity.CreationDate;
                entity.Password = existingEntity.Password;
                entity.AccessFailedCount = existingEntity.AccessFailedCount;
                entity.CorporateReason = existingEntity.CorporateReason;
                await _collection.ReplaceOneAsync(filter, entity);
            }
            else
            {
                // Lógica para lidar com o caso em que a entidade não foi encontrada
                // Você pode lançar uma exceção, fazer algo diferente, etc.
                // Neste exemplo, estou apenas registrando uma mensagem.
                Console.WriteLine($"Usuário com CNPJ {id} não encontrada.");
            }
        }
    }
}