using Domain.Interfaces.IGenerics;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IDemandServices
    {
        Task AddDemand(string demandId, string observation, DateTime date, string productId);

        Task UpdateDemand(Demand objeto);

        Task<List<Demand>> ListDemand();

        //Task<bool> VerifyCard(Demand objeto); => método a ser implementado para trazer lista de pedido referente a uma empresa

        Task DeleteDemand (string DemandId);

        Task<Demand> GetByEntityId(string demandId);

    }
}
