using AutoMapper;
using Domain.DomainNewOrderApi.InterfacesNewOrderApi.IRepositorys;
using Domain.DomainNewOrderApi.InterfacesNewOrderApi.IServices;
using Domain.Utils.HttpStatusExceptionCustom;
using Entities.Entities;

namespace Domain.DomainNewOrderApi.ServicesNewOrderApi
{
    public class NewOrderServices : INewOrderServices
    {
        private readonly IRepositoryNewOrder _IrepositoryNewOrder;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public NewOrderServices(IRepositoryNewOrder IrepositoryNewOrder, IMapper mapper, HttpClient httpClient)
        {
            _IrepositoryNewOrder = IrepositoryNewOrder;
            _mapper = mapper;
            _httpClient = httpClient;
        }
        public async Task<NewOrder> GetByEntityId(Guid idNewOrder)
        {
            if (string.IsNullOrWhiteSpace(idNewOrder.ToString()))
            {
                throw new HttpStatusExceptionCustom(StatusCodeEnum.NotAcceptable, "Order Id é obrigatório.");
            }

            var getDemand = await _IrepositoryNewOrder.GetById(idNewOrder);

            return getDemand;
        }

        public async Task<List<NewOrder>> GroupAndAnalyzeOrdersByProximity(double maxDistanceInMeters)
        {
            var list = await _IrepositoryNewOrder.GetOrdersByProximity2(1,1,1);
            return list;
        }

        public async Task<List<NewOrder>> ListNewOrder()
        {
            var list = await _IrepositoryNewOrder.GetAll();
            return list;
        }
    }
}