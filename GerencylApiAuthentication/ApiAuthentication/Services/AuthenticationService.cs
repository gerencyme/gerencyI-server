using ApiAuthentication.Models;
using ApiAuthentication.Services.Interfaces.InterfacesServices;
using ApiAuthentication.Token;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text;
using WebAPIs.Token;

namespace ApiAuthentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<GerencylRegister> _signInManager;
        private readonly UserManager<GerencylRegister> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(SignInManager<GerencylRegister> signInManager, UserManager<GerencylRegister> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<string> CriarTokenAsync(string cnpj, string senha)
        {
            if (string.IsNullOrWhiteSpace(cnpj) || string.IsNullOrWhiteSpace(senha))
            {
                throw new UnauthorizedAccessException("CNPJ e senha são obrigatórios.");
            }

            var resultado = await _signInManager.PasswordSignInAsync(
                cnpj,
                senha,
                false,
                lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                var userCurrent = await _userManager.FindByNameAsync(cnpj);
                string idUsuario = userCurrent.Id;

                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create(_jwtSettings.SecurityKey))
                    .AddSubject("Empresa - GerencyI")
                    .AddIssuer(_jwtSettings.Issuer)
                    .AddAudience(_jwtSettings.Audience)
                    .AddClaim("idUsuario", idUsuario)
                    .AddExpiry(5)
                    .Builder();

                return token.value;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<GerencylRegisterView> ReturnUser(GerencylRegisterView returnUser)
        {
            if (returnUser == null)
            {

                return null;
            }

            var user = new GerencylRegister
            {
                CNPJ = returnUser.CNPJ,
                Senha = returnUser.Senha,
                PhantasyName = returnUser.PhantasyName,
                Name = returnUser.Name,
                Email = returnUser.Email,
            };

            if (_userManager == null)
            {
                return null;
            }

            var recupera = await _userManager.FindByNameAsync(user.CNPJ);

            if (recupera == null)
            {
                return null;
            }

            var converte = new GerencylRegisterView
            {
                CNPJ = recupera.CNPJ,
                Senha = recupera.Senha,
                Name = recupera.Name,
                PhantasyName = recupera.PhantasyName,
                Email = recupera.Email,
                ConfirmSenha = recupera.Senha,
            };

            return converte;
        }

        public async Task<string> AdicionarUsuarioAsync(GerencylRegisterView register)
        {
            if (string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Senha))
            {
                return "Falta alguns dados";
            }

            var user = new GerencylRegister
            {
                Email = register.Email,
                Senha = register.Senha,
                Name = register.Name,
                CNPJ = register.CNPJ,
                PhantasyName = register.PhantasyName,
                CreationDate = DateTime.Now,
                UserName = register.CNPJ
            };

            var resultado = await _userManager.CreateAsync(user, register.Senha);

            if (resultado.Errors.Any())
            {
                return string.Join(", ", resultado.Errors.Select(e => e.Description));
            }

            // Geração de Confirmação caso precise
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno email 
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultado2 = await _userManager.ConfirmEmailAsync(user, code);

            if (resultado2.Succeeded)
            {
                return "Usuário Adicionado com Sucesso";
            }
            else
            {
                return "Erro ao confirmar usuários";
            }
        }
    }
}
