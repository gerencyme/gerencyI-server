using Domain.Interfaces.IGeneric;
using Entities;

namespace Domain.Interfaces.IRepositorys
{
    public interface IRepositoryDemand : IGenericMongoDb<Demand>
    {
        
    }
}
