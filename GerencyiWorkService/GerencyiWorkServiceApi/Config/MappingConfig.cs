using AutoMapper;
using Domain.DomainNewOrderApi.ViewsNewOrderApi;
using Entities.Entities;

namespace GerencyINewOrderApi.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<NewOrder, NewOrderUpdateView>();
                config.CreateMap<NewOrderUpdateView, NewOrder>();
                config.CreateMap<NewOrder, NewOrderAddView>();
                config.CreateMap<NewOrderAddView, NewOrder>();
                config.CreateMap<OrderCardView, NewOrder>();
                config.CreateMap<NewOrder, OrderCardView>();
            });
            return mappingConfig;
        }
    }
}
