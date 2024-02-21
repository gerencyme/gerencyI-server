using Domain.DomainNewOrderApi.InterfacesNewOrderApi.IGeneric;
using Entities.Entities;

namespace Domain.DomainNewOrderApi.InterfacesNewOrderApi.IRepositorys
{
    public interface IRepositoryNewOrder : IGenericMongoDb<NewOrder>
    {
        Task<List<NewOrder>> GetOrdersByProximity2(double latitudeA, double longitudeA, double maxDistanceInMeters);
    }
}
