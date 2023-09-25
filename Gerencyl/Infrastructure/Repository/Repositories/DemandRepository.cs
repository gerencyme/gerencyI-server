using Domain.Interfaces.IRepositorys;
using Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository.Repositories
{
    public class DemandRepository : RepositoryGeneric<Demand>, IRepositoryDemand
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public DemandRepository()
        {

            _OptionsBuilder = new DbContextOptions<ContextBase>();

        }

        public async Task<List<Demand>> ListDemand(Expression<Func<Demand, bool>> exDemand)
        {
            using var banco = new ContextBase(_OptionsBuilder);
            return await banco.Demands.Where(exDemand).AsNoTracking().ToListAsync();
        }


        /*public Task UpdateDemandObservation(int demandId, string observation)
        {
            using var banco = new ContextBase(_OptionsBuilder);
            var recupera = banco.Demand.(demandId, observation);

            throw new NotImplementedException();
        }*/
    }
}
