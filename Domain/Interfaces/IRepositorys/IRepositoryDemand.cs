using Domain.Interfaces.IGenerics;
using Entities.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces.IRepositorys
{
    public interface IRepositoryDemand : IGeneric<Demand>
    {
        Task<List<Demand>> ListDemand(Expression<Func<Demand, bool>> exDebitCard);
        //Task UpdateDemandObservation(int demandId, string observation);

    }
}
