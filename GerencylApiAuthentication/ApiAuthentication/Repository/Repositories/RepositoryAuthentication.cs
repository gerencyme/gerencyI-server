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
        private readonly IClientSessionHandle _session;

        public RepositoryAuthentication(IOptions<MongoDbSettings> settings) : base(settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<GerencylRegister>("Users");
            _session = client.StartSession();
        }

        public async Task AdicionarUsuarioAsync(GerencylRegister user)
        {
            try
            {
                _session.StartTransaction();
                await _collection.InsertOneAsync(user);
                await _session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _session.AbortTransactionAsync();
                throw new Exception("Erro ao adicionar usuário.");
            }
        }

        public async Task UpdateNewOrder(GerencylRegister entity, string id)
        {
            var filter = Builders<GerencylRegister>.Filter.Eq("CNPJ", id);
            try
            {
                if (filter != null)
                {
                    _session.StartTransaction();

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

                    await _session.CommitTransactionAsync();
                }
                else
                {
                    Console.WriteLine($"Usuário com CNPJ {id} não encontrada.");
                }
            }
            catch (Exception)
            {
                await _session.AbortTransactionAsync();
                throw new Exception("Erro ao atualizar usuário. Todas as atualizações foram revertidas.");
            }
        }

        public async Task<GerencylRegister> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var recupera = await _collection.Find(r => r.RefreshToken == refreshToken).FirstOrDefaultAsync();

            return recupera;
        }


        public async Task SaveRefreshTokenAsync(string cnpj, string refreshToken)
        {
            var filter = Builders<GerencylRegister>.Filter.Eq("CNPJ", cnpj);

            try
            {
                var userExistenceCheck = await _collection.CountDocumentsAsync(filter);
                if (userExistenceCheck > 0)
                {
                    _session.StartTransaction();

                    var updateDefinition = Builders<GerencylRegister>.Update
                    .Set(u => u.RefreshToken, refreshToken);

                    var updateResult = await _collection.UpdateOneAsync(filter, updateDefinition);


                    if (updateResult.MatchedCount == 0)
                    {
                        Console.WriteLine($"Nenhum documento encontrado para atualização para o CNPJ {cnpj}.");
                    }
                }
                else
                {
                    Console.WriteLine($"Usuário com CNPJ {cnpj} não encontrada.");
                }

                await _session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _session.AbortTransactionAsync();
                throw new Exception("Erro ao atualizar usuário. Todas as atualizações foram revertidas.");
            }
        }

        public async Task<bool> VerifyUserAsync(string cnpj, string email)
        {
            var filter = Builders<GerencylRegister>.Filter.Eq(u => u.CNPJ, cnpj);
            var filter2 = Builders<GerencylRegister>.Filter.Eq(u => u.Email, email);
            var userExists = await _collection.Find(filter).AnyAsync();
            var userExists2 = await _collection.Find(filter2).AnyAsync();

            if (userExists || userExists2)
            {
                return true;
            }
            else
                return false;
        }

        public async Task<GerencylRegister> ReturnUser(string cnpj)
        {
            try
            {
                _session.StartTransaction();
                var recupera = await _collection.Find(r => r.CNPJ == cnpj).FirstOrDefaultAsync();

                await _session.CommitTransactionAsync();

                return recupera;
            }
            catch (Exception)
            {
                await _session.AbortTransactionAsync();
                throw new Exception("Erro ao recuperar o usuário.");
            }
        }
        /*public async Task<string> GetRefreshTokenAsync(Guid userId)
        {
            // Lógica para obter o refresh token do usuário no seu armazenamento de dados
            // Exemplo fictício usando MongoDB
            var user = await _collection.Find(u => u.Id == userId).FirstOrDefaultAsync();
            return user?.RefreshToken;
        }*/
    }
}