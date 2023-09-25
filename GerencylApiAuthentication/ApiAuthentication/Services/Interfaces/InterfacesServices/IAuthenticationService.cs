using ApiAuthentication.Models;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuthentication.Services.Interfaces.InterfacesServices
{
    public interface IAuthenticationService
    {
        Task<string> CriarTokenAsync(string cnpj, string senha);

        Task<string> AdicionarUsuarioAsync(GerencylRegisterView register);

        Task<GerencylRegisterView> ReturnUser(GerencylRegisterView returnUser);
    }
}
