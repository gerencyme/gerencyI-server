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

            if (filter != null)
            {
                var updateDefinition = Builders<GerencylRegister>.Update
                .Set(u => u.Telephone, entity.Telephone)
                .Set(u => u.CompanyImg, entity.CompanyImg)
                .Set(u => u.ZipCode.Street, entity.ZipCode.Street)
                .Set(u => u.ZipCode.State, entity.ZipCode.State)
                .Set(u => u.ZipCode.City, entity.ZipCode.City)
                .Set(u => u.ZipCode.Code, entity.ZipCode.Code)
                .Set(u => u.ZipCode.Complement, entity.ZipCode.Complement)
                .Set(u => u.ZipCode.Country, entity.ZipCode.Country)
                .Set(u => u.ZipCode.Number, entity.ZipCode.Number)
                .Set(u => u.Supplier.Endereco, entity.Supplier.Endereco)
                .Set(u => u.Supplier.Nome, entity.Supplier.Nome)
                .Set(u => u.Supplier.SupplierId, entity.Supplier.SupplierId)
                .Set(u => u.Supplier.Telephone, entity.Supplier.Telephone)
                .Set(u => u.Supplier.Cnpj, entity.Supplier.Cnpj)
                .Set(u => u.Supplier.Email, entity.Supplier.Email);

                var updateResult = await _collection.UpdateOneAsync(filter, updateDefinition);
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