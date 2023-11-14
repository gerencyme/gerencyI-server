using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entities
{
    public class Product
    {
        [BsonId]
        public ObjectId ProductId { get; set; }

        public string ProductName { get; set; }

        public string DescriptionProduct { get; set; }

        public int Stock { get; set; }

        public int UnitPrice { get; set; }

        public ObjectId CompanyId { get; set; }

        //public List<DemandProduct> DemandProducts { get; set; } = new List<DemandProduct>();

        public Product()
        {
            ProductId = ObjectId.GenerateNewId();

        }

        public void AddProduct(ObjectId productId, string productName, string descriptionProduct)
        {
            ProductId = productId;
            ProductName = productName;
            DescriptionProduct = descriptionProduct;
        }

    }
}
