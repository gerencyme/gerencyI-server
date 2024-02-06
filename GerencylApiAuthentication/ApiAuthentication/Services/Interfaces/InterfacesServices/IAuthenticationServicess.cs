using ApiAuthentication.Models;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuthentication.Services.Interfaces.InterfacesServices
{
    public interface IAuthenticationServicess
    {
        Task<GerencylFullRegisterView> CriarTokenAsync(string cnpj, string senha);

        Task<GerencylFullRegisterView> AdicionarUsuarioAsync(GerencylRegisterView register);

        Task<string> UpdateUserAsync(GerencylFullRegisterView register);

        Task<GerencylFullRegisterView> ReturnUser(string cnpj);

        Task<bool> VerifyUserAsync(string cnpj, string email);

        Task<string> RefreshTokenAsync(string refreshToken);
    }
}
