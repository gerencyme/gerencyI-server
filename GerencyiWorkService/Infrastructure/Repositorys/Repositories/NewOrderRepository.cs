using Domain.DomainNewOrderApi.InterfacesNewOrderApi.IRepositorys;
using Entities;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Infrastructure.Repository.Repositories
{
    public class NewOrderRepository : RepositoryMongoDBGeneric<NewOrder>, IRepositoryNewOrder
    {
        private readonly IMongoCollection<NewOrder> _collection;
        private readonly IClientSessionHandle _session;

        public NewOrderRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<NewOrder>(typeof(NewOrder).Name);
            _session = client.StartSession();
        }

        private const string CNPJ = "CompanieCNPJ";
        private const string ORDERID = "OrderId";

        public async Task<NewOrder> GetByIdNewOrder(Guid id)
        {
            var filter = Builders<NewOrder>.Filter.Eq(ORDERID, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<NewOrder>> GetOrdersByProximity2(double latitudeA, double longitudeA, double maxDistanceInMeters)
        {
            var pipeline = new BsonDocument[]
            {
        // Filtra por pedidos em análise
        new BsonDocument("$match", new BsonDocument("OrderStatus", "Em análise")),
        
        // Adiciona um novo campo 'distancia' calculando a distância entre as coordenadas
        new BsonDocument("$addFields", new BsonDocument
        {
            { "distancia", new BsonDocument("$geoDistance", new BsonDocument
                {
                    { "distanceField", "distancia" },
                    { "near", new BsonDocument
                        {
                            { "type", "Point" },
                            { "coordinates", new BsonArray(new[] { longitudeA, latitudeA }) }
                        }
                    },
                    { "spherical", true },
                    { "maxDistance", maxDistanceInMeters }
                })
            }
        }),

        // Filtra por pedidos dentro da distância máxima
        new BsonDocument("$match", new BsonDocument("distancia", new BsonDocument("$lte", maxDistanceInMeters))),

        // Agrupa por local e produto
        new BsonDocument("$group", new BsonDocument
        {
            { "_id", new BsonDocument("$combine", new BsonArray(new BsonValue[]
                {
                    new BsonDocument("$arrayElemAt", new BsonArray(new BsonValue[] { "$Product.ProductName", 0 })),
                    new BsonDocument("$geoNear", new BsonDocument
                    {
                        { "near", new BsonDocument
                            {
                                { "type", "Point" },
                                { "coordinates", new BsonArray(new[] { "$Location.Longitude", "$Location.Latitude" }) }
                            }
                        },
                        { "distanceField", "distancia" },
                        { "spherical", true },
                        { "maxDistance", maxDistanceInMeters }
                    })
                }))
            },
            { "Pedidos", new BsonDocument("$push", "$$ROOT") }
        }),

        // Projeta o resultado final
        new BsonDocument("$project", new BsonDocument
        {
            { "_id", 0 },
            { "Local", "$_id.1" },
            { "Pedidos", 1 }
        })
            };

            var result = await _collection.Aggregate<NewOrder>(pipeline).ToListAsync();


            Console.WriteLine(result);


            return result;
        }

        private async Task UpdateOrderStatus(int orderId, string newStatus)
        {
            var filter = Builders<NewOrder>.Filter.Eq(ORDERID, orderId);
            var update = Builders<NewOrder>.Update.Set("StatusOrder", newStatus);
            await _collection.UpdateOneAsync(filter, update);
        }

        private async Task<List<NewOrder>> GetOrderDetails(int orderId)
        {
            var filter = Builders<NewOrder>.Filter.Eq(ORDERID, orderId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<NewOrder>> GetAllAnalyse()
        {
            const string underAnalyse = "underAnalyse";

            var filter = Builders<NewOrder>.Filter.Eq("StatusOrder", underAnalyse);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }
        public async Task<List<NewOrder>> GetByProximyt()
        {

            var list = await GetAllAnalyse();

            foreach (var item in list)
            {

            }
            


            return null;
        }

        static double CalculateProximity(double latitudeA, double latitudeB, double longitudeA, double longitudeB)
        {
            double radiansConversion = Math.PI / 180.0;

            double acosParam = Math.Sin(Math.PI * latitudeA * radiansConversion) * Math.Sin(Math.PI * latitudeB * radiansConversion) +
                               Math.Cos(Math.PI * latitudeA * radiansConversion) * Math.Cos(Math.PI * latitudeB * radiansConversion) *
                               Math.Cos(Math.PI * (longitudeB - longitudeA) * radiansConversion);

            double proximity = Math.Acos(acosParam) * 6378137;

            return proximity;
        }

    }

}

//var result = await _collection.Find(_ => true).ToListAsync();

/*public async Task<List<NewOrder>> GroupAndAnalyzeOrdersByProximity(double maxDistanceInMeters)
        {
            
            maxDistanceInMeters = 25000;
            var listNewOrder = new List<NewOrder>();
            var orders = await GetSimilarOrdersUnderAnalysisFromList(listNewOrder);

            var groupedOrders = new List<NewOrder>();

            foreach (var order in orders)
            {
                // Filtrar pedidos semelhantes por proximidade
                var nearbyOrders = await GetOrdersByProximity(order.Location.Latitude, order.Location.Longitude, maxDistanceInMeters);

                // Adicionar lógica de comparação e agrupamento de pedidos aqui
                // Neste exemplo, estou apenas adicionando todos os pedidos semelhantes e próximos em um único grupo
                //groupedOrders.AddRange(similarOrders);
                groupedOrders.AddRange(nearbyOrders);
            }

            return groupedOrders;
        }*/


/*         public async Task<List<Product>> GetTop10Products()
        {
            var pipeline = new BsonDocument[]
        {
        new BsonDocument
        {
            { "$unwind", "$Product" }
        },
        new BsonDocument
        {
            { "$group", new BsonDocument
                {
                    { "totalPedidos", new BsonDocument("$sum", "$Product.Quantity") },
                    { "ProductName", new BsonDocument("$first", "$Product.ProductName") },
                    { "ProductBrand", new BsonDocument("$first", "$Product.ProductBrand") },
                    { "ProductType", new BsonDocument("$first", "$Product.ProductType") },
                    { "Quantity", new BsonDocument("$first", "$Product.Quantity") },
                    { "LastTotalPrice", new BsonDocument("$first", "$Product.LastTotalPrice") }
                }
            }
        },
        new BsonDocument
        {
            { "$sort", new BsonDocument("totalPedidos", -1) }
        },
        new BsonDocument
        {
            { "$limit", 10 }
        }
        };

            return await _collection.Aggregate<Product>(pipeline).ToListAsync();
        }
 */

/*public async Task<List<DailyOrderTotal>> GetLast7DaysOrderTotal(string cnpj)
{
    var endDate = DateTime.UtcNow;  // Data atual
    var startDate = endDate.AddDays(-7);  // Retrocede 7 dias

    var filter = Builders<NewOrder>.Filter.And(
        Builders<NewOrder>.Filter.Eq("CompanieCNPJ", cnpj),
        Builders<NewOrder>.Filter.Gte("OrderDate", startDate),
        Builders<NewOrder>.Filter.Lte("OrderDate", endDate)
    );

    var group = new BsonDocument
{
{
    "$group", new BsonDocument
    {
        { "_id", new BsonDocument("$dateToString", new BsonDocument("format", "%Y-%m-%d").Add("date", "$OrderDate")) },
        { "totalOrders", new BsonDocument("$sum", 1) }
    }
}
};

    var sort = Builders<BsonDocument>.Sort.Ascending("_id");

    var pipeline = PipelineDefinition<NewOrder, DailyOrderTotal>.Create(
        new IPipelineStageDefinition[]
        {
    PipelineStageDefinitionBuilder.Match(filter),
    PipelineStageDefinitionBuilder.Group(group),
    PipelineStageDefinitionBuilder.Sort(sort)
        }
    );

    var result = await _collection.Aggregate(pipeline).ToListAsync();

    var dailyOrderTotals = result.Select(doc => new DailyOrderTotal
    {
        Date = DateTime.Parse(doc["_id"].AsString),
        TotalOrders = doc["totalOrders"].AsInt32
    }).ToList();

    return dailyOrderTotals;
}*/



/*public async Task<List<NewOrder>> GetSimilarOrdersUnderAnalysis(string cnpj, string orderId)
        {
            // Encontrar o pedido original
            var originalOrderFilter = Builders<NewOrder>.Filter.And(
                Builders<NewOrder>.Filter.Eq("CompanieCNPJ", cnpj),
                Builders<NewOrder>.Filter.Eq("OrderId", orderId)
            );

            var originalOrder = await _collection.Find(originalOrderFilter).FirstOrDefaultAsync();

            if (originalOrder == null)
            {
                // Pedido original não encontrado
                return new List<NewOrder>();
            }

            // Encontrar pedidos com os mesmos produtos e status "underAnalysis"
            var similarOrdersFilter = Builders<NewOrder>.Filter.And(
                Builders<NewOrder>.Filter.ElemMatch("Product", Builders<Product>.Filter.Eq("ProductName", originalOrder.Product.ProductName)),
                Builders<NewOrder>.Filter.Eq("StatusOrder", "underAnalysis")
                //Builders<NewOrder>.Filter.Ne("OrderId", orderId)  // Excluir o pedido original
            );

            var sort = Builders<NewOrder>.Sort.Descending("OrderDate");

            return await _collection.Find(similarOrdersFilter)
                                   .Sort(sort)
                                   .ToListAsync();
        }*/