using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiAuthentication.Models
{
    public class ContextMongoDb
    {
        private readonly IMongoDatabase _database;

        public ContextMongoDb(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        // Adicione propriedades para as coleções do Identity
        public IMongoCollection<IdentityUser> Users => _database.GetCollection<IdentityUser>("Users");
        public IMongoCollection<IdentityRole> Roles => _database.GetCollection<IdentityRole>("Roles");
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
