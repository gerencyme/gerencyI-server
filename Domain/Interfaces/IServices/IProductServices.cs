using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IProductServices
    {
        Product GetProductById(int productId);
        void CreateProduct(int productId, string observation);
        void UpdateProduct(int demandId, string observation);
    }
}
