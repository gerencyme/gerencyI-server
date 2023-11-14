using Entities;
using MongoDB.Bson;

namespace Domain.Interfaces.IServices
{
    public interface IDemandServices
    {
        Task AddDemand(ObjectId demandId, string observation, DateTime date);

        Task UpdateDemand(Demand objeto);

        Task<List<Demand>> ListDemand();

        Task DeleteDemand(Demand Demand);

        Task<Demand> GetByEntityId(ObjectId demandId);

    }
}
