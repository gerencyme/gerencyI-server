using ApiAuthentication.Models;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuthentication.Services.Interfaces.InterfacesServices
{
    public interface IAuthenticationService
    {
        Task<string> CriarTokenAsync(string cnpj, string senha);

        Task<string> CriarTokenTeste(string cnpj, string senha);

        Task<string> AdicionarUsuarioAsync(GerencylRegisterView register);

        Task<string> AdicionarUsuarioTeste(GerencylRegisterView register);

        Task<GerencylRegister> ReturnUser(string cnpj);
    }
}
