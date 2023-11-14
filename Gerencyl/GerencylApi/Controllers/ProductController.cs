using AutoMapper;
using Domain.Interfaces.IServices;
using Entities;
using GerencylApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GerencylApi.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IMapper _IMapper;
        private readonly IProductServices _IProductServices;
        //private readonly IServiceCollection _serviceCollection;

        public ProductController(IMapper IMapper, IProductServices IProductServices)
        {
            _IProductServices = IProductServices;
            _IMapper = IMapper;
        }


        [Produces("application/json")]
        [HttpPost("/api/AddProduct")]
        public async Task<IActionResult> Add([FromQuery] Product product)
        {
            await _IProductServices.AddProduct(product.ProductId, product.ProductName, product.DescriptionProduct);

            return Ok();

        }

    }
}
