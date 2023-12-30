using ApiAuthentication.Models;
using Interfaces.IGeneric;

namespace ApiAuthentication.Services.Interfaces.InterfacesRepositories
{
    public interface IAuthenticationRepository : IGenericMongoDb<GerencylRegister>
    {
        Task UpdateNewOrder(GerencylRegister entity, string id);
    }
}
