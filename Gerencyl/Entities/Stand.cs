using Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entities
{
    public class Stand
    {
        [BsonId]
        public ObjectId StandId { get; set; }

        public string? NumberStand { get; set; }

        public bool StandAllReadReserved { get; set; }

        public bool UserPermission { get; set; }

        public string? CPFResponsible { get; set; }

        public decimal PaymentTotal { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

        public OrderStatus StandStatus { get; set; }

        public ObjectId CompanyId { get; set; }

        public List<Demand> Demands { get; set; } = new List<Demand>();
    }
}
