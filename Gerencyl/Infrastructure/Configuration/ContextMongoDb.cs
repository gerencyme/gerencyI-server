using Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Configuration
{
    public class ContextMongoDb
    {
        private readonly IMongoDatabase _database;

        public ContextMongoDb(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Company> Companies => _database.GetCollection<Company>("Companies");
        public IMongoCollection<Stand> Stands => _database.GetCollection<Stand>("Stands");
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Demand> Demands => _database.GetCollection<Demand>("Demands");
        public IMongoCollection<DemandProduct> DemandProducts => _database.GetCollection<DemandProduct>("DemandProducts");

    }
    public class MongoDbSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }

}

