using MongoDB.Bson;

namespace Domain.Interfaces.IServices
{
    public interface IProductServices
    {
        Task AddProduct(ObjectId productId, string productName, string descriptionProduct);

    }
}
