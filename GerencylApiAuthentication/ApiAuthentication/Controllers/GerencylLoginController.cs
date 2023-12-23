using ApiAuthentication.Services.Interfaces.InterfacesServices;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuthentication.Controllers
{
    [ApiController]
    [Authorize]
    public class GerencylLoginController : ControllerBase
    {
        private readonly IAuthenticationServicess _authenticationService;
        public GerencylLoginController(IAuthenticationServicess authenticationService)
        {
            _authenticationService = authenticationService;
        }


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
                else return NotFound("não foi possível localizar o usúario");
            }
            catch (HttpStatusExceptionCustom ex)
            {

                return StatusCode(ex.StatusCode, ex.Message);

            }
        }

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
                return Unauthorized("Senha ou CNPJ inválido");
            }
        }

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
