using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entities
{
    public class DemandProduct
    {
        [BsonId]
        public ObjectId DemandProductId { get; set; }

        public ObjectId DemandId { get; set; }

        public ObjectId ProductId { get; set; }

        public int Quantity { get; set; }

        public ObjectId CompanyId { get; set; }

    }
}
