using Domain.Interfaces.IRepositorys;
using Domain.Interfaces.IServices;
using Entities;
using MongoDB.Bson;

namespace Domain.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IRepositoryProduct _IrepositoryProduct;
        public ProductServices(IRepositoryProduct IrepositoryProduct)
        {
            _IrepositoryProduct = IrepositoryProduct;
        }

        public async Task AddProduct(ObjectId productId, string productName, string descriptionProduct)
        {
            var newProduct = new Product();
            newProduct.AddProduct(productId, productName, descriptionProduct);
            await _IrepositoryProduct.Add(newProduct);

            return;
        }
    }
}
