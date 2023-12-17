using ApiAuthentication.Services.Interfaces.InterfacesServices;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuthentication.Controllers
{
    [ApiController]
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


        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ReturnUser")]
        public async Task<IActionResult> ReturnUser([FromBody] string cnpj)
        {
            var user = await _authenticationService.ReturnUser(cnpj);
            try
            {
                if (user != null)
                {
                    return Ok(user);
                }
                else return BadRequest("não foi possível localizar o usúario");
            }
            catch (HttpStatusExceptionCustom ex)
            {

                return StatusCode(ex.StatusCode, ex.Message);

            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/GenereateTokenIdentity")]
        public async Task<IActionResult> CriarTokenIdentity([FromBody] GerencylLoginView login)
        {

            try
            {
                var token = await _authenticationService.CriarTokenAsync(login.CNPJ, login.Password);
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
        public async Task<IActionResult> AddUserIdentity([FromBody] GerencylRegisterView register)
        {
            try
            {
                var result = await _authenticationService.AdicionarUsuarioAsync(register);
                return Ok(result);
            }
            catch (HttpStatusExceptionCustom ex)
            {

                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/UpdateUserIdentity")]
        public async Task<IActionResult> UpdateUserIdentity([FromBody] GerencylFullRegisterView register)
        {
            try
            {
                var result = await _authenticationService.UpdateUserAsync(register);
                return Ok(result);
            }
            catch (HttpStatusExceptionCustom ex)
            {

                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

    }
}
