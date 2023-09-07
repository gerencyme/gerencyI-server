using ApiAuthentication.Services.Interfaces.InterfacesServices;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuthentication.Controllers
{
    public class GerencylLoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
        public GerencylLoginController(IAuthenticationService authenticationService,
            IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/GenereateTokenIdentity")]
        public async Task<IActionResult> CriarTokenIdentity([FromQuery] GerencylLoginView login)
        {

            try
            {
                var token = await _authenticationService.CriarTokenAsync(login.CNPJ, login.Senha);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AddUserIdentity")]
        public async Task<IActionResult> AdicionaUsuarioIdentity([FromQuery] GerencylRegisterView register)
        {
            var result = await _authenticationService.AdicionarUsuarioAsync(register);
            return Ok(result);
        }

    }
}
