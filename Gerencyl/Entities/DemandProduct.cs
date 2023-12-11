using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entities
{
    public class DemandProduct
    {
        [BsonId]
        public ObjectId DemandProductId { get; set; }
        public string? NumberDemandProduct { get; init; }
        public int Quantity { get; set; } // Quantidade de produtos selecionados
        public decimal TotalValue { get; set; } // Valor total do produto * quantidade
        public ObjectId ProductId { get; set; }
        public Product? Product { get; set; } // Referência ao produto associado
        public ObjectId CompanyId { get; set; }

    }
}
