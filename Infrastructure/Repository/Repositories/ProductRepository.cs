using Entities.Entities;
using Infrastructure.Configuration;
using Infrastucture.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Repositories
{
    public class ProductRepository : RepositoryGeneric<Product>
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public ProductRepository()
        {

            _OptionsBuilder = new DbContextOptions<ContextBase>();

        }
    }
}
