using ApiAuthentication.Models;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebAPIs.Token;

namespace ApiAuthentication.Controllers
{
    public class GerencylLoginController : ControllerBase
    {
        private readonly UserManager<GerencylRegister> _userManager;
        private readonly SignInManager<GerencylRegister> _signInManager;
        private readonly IConfiguration _configuration;
        public GerencylLoginController(UserManager<GerencylRegister> userManager,
            SignInManager<GerencylRegister> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/GenereateTokenIdentity")]
        public async Task<IActionResult> CriarTokenIdentity([FromQuery] GerencylLoginView login)
        {
            if (string.IsNullOrWhiteSpace(login.CNPJ) || string.IsNullOrWhiteSpace(login.Senha))
            {
                return Unauthorized();
            }

            var resultado = await
                _signInManager.PasswordSignInAsync(
                    login.CNPJ,
                    login.Senha,
                    false,
                    lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                // Recupera Usuário Logadomonte
                var userCurrent = await _userManager.FindByIdAsync(login.CNPJ);
                var idUsuario = userCurrent.CNPJ;

                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("Empresa - Canal Dev Net Core")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddClaim("idUsuario", idUsuario)
                .AddExpiry(5)
                .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }

        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AddUserIdentity")]
        public async Task<IActionResult> AdicionaUsuarioIdentity([FromQuery] GerencylRegisterView login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return Ok("Falta alguns dados");


            var user = new GerencylRegister
            {
                Email = login.Email,
                Senha = login.Senha,
                Name  = login.Name,
                CNPJ = login.CNPJ,
                PhantasyName = login.PhantasyName,
                CreationDate = DateTime.Now,
            };

            var resultado = await _userManager.CreateAsync(user, login.Senha);

            if (resultado.Errors.Any())
            {
                return Ok(resultado.Errors);
            }



            // Geração de Confirmação caso precise
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno email 
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultado2 = await _userManager.ConfirmEmailAsync(user, code);

            if (resultado2.Succeeded)
                return Ok("Usuário Adicionado com Sucesso");
            else
                return Ok("Erro ao confirmar usuários");

        }




    }
}
