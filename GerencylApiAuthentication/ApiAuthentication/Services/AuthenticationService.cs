using ApiAuthentication.Models;
using ApiAuthentication.Services.Interfaces.InterfacesServices;
using ApiAuthentication.Token;
using ApiAuthentication.Views;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
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
        private readonly IMapper _mapper;
        private readonly EmailConfirmationService _sendEmaail;

        public AuthenticationService(SignInManager<GerencylRegister> signInManager, UserManager<GerencylRegister> userManager,
            ISendGridClient sendGridClient, IOptions<JwtSettings> jwtSettings,
            IConfiguration configuration, List<GerencylRegister> usuarios,
            EmailConfirmationService sendEmaail, IMapper mapper)
        {
            _mapper = mapper;
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

        public async Task<GerencylFullRegisterView> ReturnUser(string cnpj)
        {

            var recupera =  _usuarios.Find(r => r.CNPJ == cnpj);

            if (recupera == null)
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.NotFound);
            }

            var user = _mapper.Map<GerencylFullRegisterView>(recupera);

            return user;
        }

        public async Task<string> AdicionarUsuarioAsync(GerencylRegisterView register)
        {
            var verifica = await verifyUser(register.CNPJ);

            if (verifica == true)
            {
                //return "usuario já existe";
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.Conflict);
            }
            if (string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Password))
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.BadRequest);
            }
            if (register.ConfirmPassword != register.Password)
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.NotAcceptable);
            }

            var user = _mapper.Map<GerencylRegister>(register);

            var resultado = await _userManager.CreateAsync(user, register.Password);

            if (resultado.Errors.Any())
            {
                return string.Join(", ", resultado.Errors.Select(e => e.Description));
            }
            else
            {
                var confirmationLink = await _sendEmaail.GenerateConfirmRegister(user);

                await _sendEmaail.SendEmailConfirmationAsync(confirmationLink, user.Email);

                var retornaToken = await CriarTokenTeste(user.CNPJ, user.Password);

                return retornaToken.ToString();
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

            if (string.IsNullOrWhiteSpace(register.Password))
            {
                return "Senha é obrigatório!";
                //throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.BadRequest);
            }
            if (register.ConfirmPassword != register.Password)
            {
                return "confirmação de senha deve ser igual a senha";
                //throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.NotAcceptable);
            }

            var user = _mapper.Map<GerencylRegister>(register);

            _usuarios.Add(user);


            // Simula a criação do usuário (sem acessar o banco de dados real)
            var resultado = IdentityResult.Success;

            if (resultado.Errors.Any())
            {
                return string.Join(", ", resultado.Errors.Select(e => e.Description));
            }else
            {
                var confirmationLink = await _sendEmaail.GenerateConfirmRegister(user);

                await _sendEmaail.SendEmailConfirmationAsync(confirmationLink, user.Email);

                var retornaToken = await CriarTokenTeste(user.CNPJ, user.Password);

                return retornaToken.ToString();
            }

        }

            private async Task<bool> verifyUser(string CNPJ)
        {

            var userExists = _usuarios.Any(u => u.CNPJ == CNPJ);

            return userExists;
        }

    }
}