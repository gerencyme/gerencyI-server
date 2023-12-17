using ApiAuthentication.Models;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuthentication.Services.Interfaces.InterfacesServices
{
    public interface IAuthenticationService
    {
        Task<GerencylFullRegisterView> CriarTokenAsync(string cnpj, string senha);

        Task<GerencylFullRegisterView> CriarTokenTeste(string cnpj, string senha);

        Task<string> AdicionarUsuarioAsync(GerencylRegisterView register);

        Task<GerencylFullRegisterView> AdicionarUsuarioTeste(GerencylRegisterView register);

        Task<string> UpdateUserAsync(GerencylFullRegisterView register);

        Task<GerencylFullRegisterView> ReturnUser(string cnpj);
    }
}
