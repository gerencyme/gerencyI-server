using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entities
{
    public class Company
    {
        [BsonId]
        public ObjectId CompanyId { get; set; }

        public string? CompanyCod { get; set; }

        public string? NameCompany { get; set; }

        public string? CNPJ { get; set; }

        public List<Stand> Stands { get; set; } = new List<Stand>();

        public List<Product> Products { get; set; } = new List<Product>();

    }
}
