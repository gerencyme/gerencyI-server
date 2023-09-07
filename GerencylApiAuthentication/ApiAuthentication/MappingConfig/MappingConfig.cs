using ApiAuthentication.Models;
using ApiAuthentication.Views;
using AutoMapper;

namespace GerencylApi.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<GerencylRegisterView, GerencylRegister>();
                config.CreateMap<GerencylRegister, GerencylRegisterView>();
            });
            return mappingConfig;
        }
    }
}
