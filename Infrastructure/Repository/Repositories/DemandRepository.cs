using Domain.Interfaces.IGenerics;
using Domain.Interfaces.IServices;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastucture.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Repositories
{
    public class DemandRepository : RepositoryGeneric<Demand>, IDemandServices
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public DemandRepository()
        {

            _OptionsBuilder = new DbContextOptions<ContextBase>();

        }

        public async Task<List<Demand>> ListDemand(Expression<Func<Demand, bool>> exDemand)
        {
            using var banco = new ContextBase(_OptionsBuilder);
            return await banco.Demand.Where(exDemand).AsNoTracking().ToListAsync();
        }


        /*public Task UpdateDemandObservation(int demandId, string observation)
        {
            using var banco = new ContextBase(_OptionsBuilder);
            var recupera = banco.Demand.(demandId, observation);

            throw new NotImplementedException();
        }*/
    }
}
