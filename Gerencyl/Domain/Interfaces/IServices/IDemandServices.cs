using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IDemandServices
    {
        Task AddDemand(int demandId, string observation, DateTime date, int productId);

        Task UpdateDemand(Demand objeto);

        Task<List<Demand>> ListDemand();

        //Task<bool> VerifyCard(Demand objeto); => método a ser implementado para trazer lista de pedido referente a uma empresa

        Task DeleteDemand(int DemandId);

        Task<Demand> GetByEntityId(int demandId);

    }
}
