using ApiAuthentication.Models;
using Interfaces.IGeneric;

namespace ApiAuthentication.Services.Interfaces.InterfacesRepositories
{
    public interface IAuthenticationRepository : IGenericMongoDb<GerencylRegister>
    {
        Task AdicionarUsuarioAsync(GerencylRegister user);
        Task UpdateNewOrder(GerencylRegister entity, string id);
        Task<GerencylRegister> ReturnUser(string cnpj);
        Task<bool> VerifyUserAsync(string cnpj, string email);
        Task<GerencylRegister> GetUserByRefreshTokenAsync(string refreshToken);
        Task SaveRefreshTokenAsync(string cnpj, string refreshToken);
    }
}
