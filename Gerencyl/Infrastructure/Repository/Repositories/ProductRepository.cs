using Domain.Interfaces.IGeneric;
using Domain.Interfaces.IRepositorys;
using Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace Infrastructure.Repository.Repositories
{
    public class ProductRepository : RepositoryMongoDBGeneric<Product>, IRepositoryProduct
    {
        public ProductRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {

        }
    }
}
