using Domain.Interfaces.IRepositorys;
using Domain.Interfaces.IServices;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class DemandServices : IDemandServices
    {
        private readonly IRepositoryDemand _IrepositoryDemand;

        public DemandServices(IRepositoryDemand IrepositoryDemand)
        {
            _IrepositoryDemand = IrepositoryDemand;
        }

        public async Task AddDemand(string demandId, string observation, DateTime date, string productId)
        {
            var newDemand = new Demand();
            newDemand.AddDemand(demandId, observation, date, productId);
            await _IrepositoryDemand.Add(newDemand);

            return;
        }

        public async Task DeleteDemand(string demandId)
        {
            var deleteDemand = await _IrepositoryDemand.GetEntityById(demandId);
            await _IrepositoryDemand.Delete(deleteDemand);
        }

        public Task<Demand> GetByEntityId(string demandId)
        {
            var getDemand = _IrepositoryDemand.GetEntityById(demandId);
            return getDemand;
        }

        public async Task<List<Demand>> ListDemand()
        {
            var list = await _IrepositoryDemand.List();
            return list;
        }

        public async Task UpdateDemand(Demand objeto)
        {
            await _IrepositoryDemand.Update(objeto);
        }
    }
}
