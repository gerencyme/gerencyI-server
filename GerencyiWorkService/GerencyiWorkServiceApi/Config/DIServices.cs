using Domain.DomainNewOrderApi.InterfacesNewOrderApi.IServices;
using Domain.DomainNewOrderApi.ServicesNewOrderApi;

namespace Domain.Utils
{
    public class DIServices
    {
        public void MapDependencies(IServiceCollection services)
        {
            // Mapeia as dependências relacionadas a NewOrder
            services.AddSingleton<INewOrderServices, NewOrderServices>();

        }
    }
}
