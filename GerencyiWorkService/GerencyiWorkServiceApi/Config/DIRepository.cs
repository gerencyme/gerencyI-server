using Domain.DomainNewOrderApi.InterfacesNewOrderApi.IRepositorys;
using Infrastructure.Repository.Repositories;

namespace GerencyIProductApi.Config
{
    public class DIRepository
    {
        public void RegisterDependencies(IServiceCollection services)
        {
            // Registra as dependências
            services.AddSingleton<IRepositoryNewOrder, NewOrderRepository>();
            //services.AddSingleton<IRepositoryProduct, ProductRepository>();
        }
    }
}
