using AutoMapper;
using Entities;
using GerencylApi.Models;

namespace GerencylApi.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<DemandModel, Demand>();
                config.CreateMap<Demand, DemandModel>();
            });
            return mappingConfig;
        }
    }
}
