using ApiAuthentication.Services.Interfaces.InterfacesServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TesteController : ControllerBase
    {

        private readonly IAuthenticationServicess _authenticationService;
        private readonly IConfiguration _configuration;

        public TesteController(IAuthenticationServicess authenticationService,
            IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [Produces("application/json")]
        [HttpPost("ReturnUser")]
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
    }
}
