using Domain.DomainNewOrderApi.ViewsNewOrderApi;
using Entities.Entities;
using MongoDB.Bson;

namespace Domain.DomainNewOrderApi.InterfacesNewOrderApi.IServices
{
    public interface INewOrderServices
    {
        Task<NewOrder> GetByEntityId(Guid idNewOrder);

        Task<List<NewOrder>> ListNewOrder();

        Task<List<NewOrder>> GroupAndAnalyzeOrdersByProximity(double maxDistanceInMeters);
    }
}
