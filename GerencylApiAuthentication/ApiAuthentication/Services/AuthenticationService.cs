﻿using ApiAuthentication.Models;
using ApiAuthentication.Services.Interfaces.InterfacesServices;
using ApiAuthentication.Token;
using ApiAuthentication.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SendGrid;
using System.Text;
using WebAPIs.Token;

namespace ApiAuthentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<GerencylRegister> _signInManager;
        private readonly UserManager<GerencylRegister> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;
        private List<GerencylRegister> _usuarios;
        private readonly EmailConfirmationService _sendEmaail;

        public AuthenticationService(SignInManager<GerencylRegister> signInManager, UserManager<GerencylRegister> userManager,
            ISendGridClient sendGridClient, IOptions<JwtSettings> jwtSettings,
            IConfiguration configuration, List<GerencylRegister> usuarios,
            EmailConfirmationService sendEmaail)
        {
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _sendGridClient = sendGridClient;
            _configuration = configuration;
            _userManager = userManager;
            _usuarios = usuarios;
            _sendEmaail = sendEmaail;
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
                    .AddExpiry(5)
                    .Builder();

                return token.value;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<string> CriarTokenTeste(string cnpj, string senha)
        {

            var verifica = await verifyUser(cnpj);

            if (verifica != true)
            {
                return "usuario não existe";
                //throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.Conflict);
            }
            if (string.IsNullOrWhiteSpace(cnpj) || string.IsNullOrWhiteSpace(senha))
            {
                throw new UnauthorizedAccessException("CNPJ e senha são obrigatórios.");
            }

            var usuario = _usuarios.FirstOrDefault(u =>
                            u.CNPJ.Equals(cnpj, StringComparison.OrdinalIgnoreCase) &&
                            u.Password.Equals(senha, StringComparison.Ordinal));

            if (usuario != null)
            {
                // Simula a autenticação (sem acessar o banco de dados real)
                var resultado = IdentityResult.Success;

                if (!string.IsNullOrEmpty(senha) && resultado.Succeeded)
                {
                    var idUsuario = usuario.Id;

                    var token = new TokenJWTBuilder()
                        .AddSecurityKey(JwtSecurityKey.Create(_jwtSettings.SecurityKey))
                        .AddSubject("Empresa - GerencyI")
                        .AddIssuer(_jwtSettings.Issuer)
                        .AddAudience(_jwtSettings.Audience)
                        .AddExpiry(60)
                        .Builder();

                    return token.value;
                }
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<GerencylRegister> ReturnUser(GerencylRegisterView returnUser)
        {
            /*if (returnUser == null)
            {

                return null;
            }


            var user = new GerencylRegister();

            if (_userManager == null)
            {
                return null;
            }*/

            var recupera = await _userManager.FindByNameAsync(returnUser.CNPJ);

            if (recupera == null)
            {
                return null;
            }

            var converte = new GerencylRegister
            {
                CNPJ = recupera.CNPJ,
                Name = recupera.Name,
                CorporateReason = recupera.CorporateReason,
                Email = recupera.Email,
            };

            return converte;
        }

        public async Task<string> AdicionarUsuarioAsync(GerencylRegisterView register)
        {
            if (string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Password.Password))
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.BadRequest);
            }
            if (register.Password.ConfirmPassword != register.Password.Password)
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.NotAcceptable);
            }

            var user = new GerencylRegister()
            {
                Email = register.Email,
                Password = register.Password.Password,
                CreationDate = DateTime.Now,
                CNPJ = register.CNPJ,
                UpdateDate = DateTime.Now,
                Name = register.Name,
                UserName = register.Name,
                CorporateReason = register.CorporateReason,

            };
            /*
            var user = new GerencylRegister(
                creationDate: returnUser.CreationDate,
                updateDate: returnUser.UpdateDate,
                email: returnUser.Email,
                password: returnUser.Password.Password,
                name: returnUser.Name,
                cnpj: returnUser.CNPJ,
                corporateReason: returnUser.CorporateReason
            );*/

            var resultado = await _userManager.CreateAsync(user, register.Password.Password);

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

        public async Task<string> AdicionarUsuarioTeste(GerencylRegisterView register)
        {

            var verifica = await verifyUser(register.CNPJ);

            if (verifica == true)
            {
                return "usuario já existe";
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.Conflict);
            }

            if (string.IsNullOrWhiteSpace(register.Password.Password))
            {
                return "Senha é obrigatório!";
                //throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.BadRequest);
            }
            if (register.Password.ConfirmPassword != register.Password.Password)
            {
                return "confirmação de senha deve ser igual a senha";
                //throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.NotAcceptable);
            }

            var user = new GerencylRegister()
            {
                Email = register.Email,
                Password = register.Password.Password,
                CreationDate = DateTime.Now,
                CNPJ = register.CNPJ,
                UpdateDate = DateTime.Now,
                Name = register.Name,
                UserName = register.Name,
                CorporateReason = register.CorporateReason,

            };

            /*var user = new GerencylRegister(
               email: register.Email,
               password: register.Password.Password,
               name: register.Name,
               cnpj: register.CNPJ,
               corporateReason: register.CorporateReason,
               creationDate: register.CreationDate,
               updateDate: register.UpdateDate
           );*/

            _usuarios.Add(user);


            var confirmationLink = await _sendEmaail.GenerateConfirmRegister(user);

            await _sendEmaail.SendEmailConfirmationAsync(confirmationLink, user.Email);


            //await _sendEmaail.SendEmailConfirmationAsync(user);

            // Simula a criação do usuário (sem acessar o banco de dados real)
            var resultado = IdentityResult.Success;

            if (resultado.Errors.Any())
            {
                return string.Join(", ", resultado.Errors.Select(e => e.Description));
            }

            return resultado.ToString();


            /*// Geração de Confirmação caso precise
            var userId = Guid.NewGuid().ToString(); // Simula um novo ID do usuário
            var code = Guid.NewGuid().ToString();   // Simula um código de confirmação

            // Simula a confirmação do email (sem acessar o banco de dados real)
            var resultado2 = IdentityResult.Success;

            if (resultado2.Succeeded)
            {
                return "Usuário Adicionado com Sucesso";
            }
            else
            {
                return "Erro ao confirmar usuários";
            }*/

        }

            private async Task<bool> verifyUser(string CNPJ)
        {

            var userExists = _usuarios.Any(u => u.CNPJ == CNPJ);

            return userExists;
        }

    }
}