﻿using ApiAuthentication.Services.Interfaces.InterfacesServices;
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


        [AllowAnonymous]
        [Produces("application/json")]
        [HttpGet("/api/ReturnUser")]
        public async Task<IActionResult> ReturnUser([FromBody] GerencylRegisterView returnUser)
        {
            var user = await _authenticationService.ReturnUser(returnUser);
            try
            {
                if (user != null)
                {
                    return Ok(user);
                }
                else return BadRequest("não foi possível localizar o usúario");
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/GenereateTokenIdentity")]
        public async Task<IActionResult> CriarTokenIdentity([FromBody] GerencylLoginView login)
        {

            try
            {
                var token = await _authenticationService.CriarTokenTeste(login.CNPJ, login.Password);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AddUserIdentityTeste")]
        public async Task<IActionResult> AdicionaUsuarioIdentity([FromBody] GerencylRegisterView register)
        {
            try
            {
                var result = await _authenticationService.AdicionarUsuarioTeste(register);
                return Ok(result);
            }
            catch (HttpStatusExceptionCustom ex)
            {

                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

    }
}
