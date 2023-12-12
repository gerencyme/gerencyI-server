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
                config.CreateMap<GerencylRegister, GerencylLoginView>();
                config.CreateMap<GerencylLoginView, GerencylRegister>();
                config.CreateMap<GerencylRegister, GerencylFullRegisterView>();
                config.CreateMap<GerencylFullRegisterView, GerencylRegister>();

            });
            return mappingConfig;
        }
    }
}
