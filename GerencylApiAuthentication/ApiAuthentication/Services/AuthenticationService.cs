using ApiAuthentication.Models;
using ApiAuthentication.Services.Interfaces.InterfacesServices;
using ApiAuthentication.Token;
using ApiAuthentication.Views;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAPIs.Token;

namespace ApiAuthentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMongoCollection<GerencylRegister> _usersCollection;
        private readonly SignInManager<GerencylRegister> _signInManager;
        private readonly UserManager<GerencylRegister> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly EmailConfirmationService _sendEmaail;

        public AuthenticationService(IMongoDatabase database, SignInManager<GerencylRegister> signInManager, UserManager<GerencylRegister> userManager,
            IOptions<JwtSettings> jwtSettings,EmailConfirmationService sendEmaail, IMapper mapper)
        {
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _userManager = userManager;
            _sendEmaail = sendEmaail;
            _usersCollection = database.GetCollection<GerencylRegister>("Users");
        }

        public async Task<GerencylFullRegisterView> CriarTokenAsync(string cnpj, string senha)
        {
            // Verifica se o usuário existe no MongoDB
            var usuario = await _usersCollection.Find(u =>
                u.CNPJ.Equals(cnpj, StringComparison.OrdinalIgnoreCase) &&
                u.PasswordHash.Equals(senha, StringComparison.Ordinal))
                .FirstOrDefaultAsync();

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

                    var returnLogin = await ReturnUser(cnpj);

                    returnLogin.Token = token.value;

                    return returnLogin;
                }
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<GerencylFullRegisterView> ReturnUser(string cnpj)
        {
            var recupera = await _usersCollection.Find(r => r.CNPJ == cnpj).FirstOrDefaultAsync();

            if (recupera == null)
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.NotFound);
            }

            var user = _mapper.Map<GerencylFullRegisterView>(recupera);

            return user;
        }

        public async Task<GerencylFullRegisterView> AdicionarUsuarioAsync(GerencylRegisterView register)
        {
            var verifica = await VerifyUserAsync(register.CNPJ);

            if (verifica == true)
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionCustom(StatusCodeEnum.Conflict);
            }
            if (string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Password))
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionCustom(StatusCodeEnum.BadRequest);
            }
            if (register.ConfirmPassword != register.Password)
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.NotAcceptable);
            }

            var user = _mapper.Map<GerencylRegister>(register);
            // Realize operações personalizadas para adicionar um usuário ao MongoDB
            // Por exemplo, configurar propriedades adicionais, manipular senhas, etc.

            user.PasswordHash = register.Password;// hash da senha, se necessário;

            user.ZipCode.Code = "88888888";

            await _userManager.UpdateSecurityStampAsync(user);

            await _usersCollection.InsertOneAsync(user);

            var confirmationLink = await _sendEmaail.GenerateConfirmRegister(user);

            await _sendEmaail.SendEmailConfirmationAsync(confirmationLink, user.Email);


            var retornaToken = await CriarTokenAsync(user.CNPJ, register.Password);

            return retornaToken;
        }

        public async Task<string> UpdateUserAsync(GerencylFullRegisterView register)
        {
            var user = _mapper.Map<GerencylRegister>(register);

            // Localize o usuário no MongoDB pelo Id
            var filter = Builders<GerencylRegister>.Filter.Eq(u => u.Id, user.Id);

            // Execute a atualização apenas se o usuário existir
            var updateResult = await _usersCollection.ReplaceOneAsync(filter, user);

            if (updateResult.ModifiedCount == 0)
            {
                throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionGeneric(StatusCodeEnum.NotFound);
            }

            return "Update User Success";
        }

        private async Task<bool> VerifyUserAsync(string cnpj)
        {
            var filter = Builders<GerencylRegister>.Filter.Eq(u => u.CNPJ, cnpj);
            var userExists = await _usersCollection.Find(filter).AnyAsync();

            return userExists;
        }

    }
}