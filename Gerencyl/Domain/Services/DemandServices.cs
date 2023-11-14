using Domain.Interfaces.IRepositorys;
using Domain.Interfaces.IServices;
using Entities;
using MongoDB.Bson;

namespace Domain.Services
{
    public class DemandServices : IDemandServices
    {
        private readonly IRepositoryDemand _IrepositoryDemand;

        public DemandServices(IRepositoryDemand IrepositoryDemand)
        {
            _IrepositoryDemand = IrepositoryDemand;
        }

        public async Task AddDemand(ObjectId demandId, string observation, DateTime date)
        {
            var newDemand = new Demand();
            newDemand.AddDemand(demandId, observation, date);
            await _IrepositoryDemand.Add(newDemand);

            return;
        }

        public async Task DeleteDemand(Demand demand)
        {
            var deleteDemand = await _IrepositoryDemand.GetById(demand.DemandId);
            await _IrepositoryDemand.Delete(deleteDemand);
        }

        public Task<Demand> GetByEntityId(ObjectId demandId)
        {
            var getDemand = _IrepositoryDemand.GetById(demandId);
            return getDemand;
        }

        public async Task<List<Demand>> ListDemand()
        {
            var list = await _IrepositoryDemand.GetAll();
            return list;
        }

        public async Task UpdateDemand(Demand objeto)
        {
            await _IrepositoryDemand.Update(objeto);
        }
    }
}
