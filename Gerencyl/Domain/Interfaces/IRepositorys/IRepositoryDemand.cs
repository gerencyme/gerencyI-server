using Domain.Interfaces.IGeneric;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositorys
{
    public interface IRepositoryDemand : IGeneric<Demand>
    {
        Task<List<Demand>> ListDemand(Expression<Func<Demand, bool>> exDebitCard);
        //Task UpdateDemandObservation(int demandId, string observation);

    }
}
