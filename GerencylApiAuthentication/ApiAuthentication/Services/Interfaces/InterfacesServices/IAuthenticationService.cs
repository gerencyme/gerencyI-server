using ApiAuthentication.Views;

namespace ApiAuthentication.Services.Interfaces.InterfacesServices
{
    public interface IAuthenticationService
    {
        Task<string> CriarTokenAsync(string cnpj, string senha);

        Task<string> AdicionarUsuarioAsync(GerencylRegisterView register);
    }
}
