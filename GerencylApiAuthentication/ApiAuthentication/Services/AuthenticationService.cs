using ApiAuthentication.Models;
using ApiAuthentication.Services.Interfaces.InterfacesRepositories;
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
    public class AuthenticationService : IAuthenticationServicess
    {
        private readonly IMongoCollection<GerencylRegister> _usersCollection;
        private readonly IAuthenticationRepository _iauthenticationRepository;
        private readonly UserManager<GerencylRegister> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly EmailConfirmationService _sendEmaail;

        public AuthenticationService(IMongoDatabase database, UserManager<GerencylRegister> userManager,
            IOptions<JwtSettings> jwtSettings, EmailConfirmationService sendEmaail, IMapper mapper,
            IAuthenticationRepository iauthenticationRepository)
        {
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _sendEmaail = sendEmaail;
            _usersCollection = database.GetCollection<GerencylRegister>("Users");
            _iauthenticationRepository = iauthenticationRepository;
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
                        .AddSubject(cnpj)
                        .AddIssuer(_jwtSettings.Issuer)
                        .AddAudience(_jwtSettings.Audience)
                        .AddClaim("user", "comum")
                        .AddExpiry(60)
                        .Builder();

                    var returnLogin = await ReturnUser(cnpj);

                    returnLogin.Token = token.Value;

                    return returnLogin;
                }
            }

            throw new UnauthorizedAccessException();
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            var tokenBuilder = new TokenJWTBuilder()
                .AddSubject(userId)  // Set the user ID as the subject
                .WithRefreshTokenExpiration(2880);  // Set refresh token expiration to 48 hours

            var refreshToken = tokenBuilder.Builder(isRefreshToken: true);

            // Store the refresh token securely (implementation not shown)

            return refreshToken.Value;
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
            var verifica = await VerifyUserAsync(register.CNPJ, register.Email);

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

        /*public async Task<string> UpdateUserAsync(GerencylFullRegisterView register)
        {
            var user = _mapper.Map<GerencylRegister>(register);

            // Localize o usuário no MongoDB pelo Id
            var filter = Builders<GerencylRegister>.Filter.Eq(u => u.Id, user.Id);

            // Crie a atualização usando o operador $set
            var updateDefinition = Builders<GerencylRegister>.Update
                .Set(u => u.Telephone, user.Telephone)
                .Set(u => u.ZipCode.Street, user.ZipCode.Street)
                .Set(u => u.ZipCode.State, user.ZipCode.State)
                .Set(u => u.ZipCode.City, user.ZipCode.City)
                .Set(u => u.ZipCode.Code, user.ZipCode.Code)
                .Set(u => u.ZipCode.Complement, user.ZipCode.Complement)
                .Set(u => u.ZipCode.Country, user.ZipCode.Country)
                .Set(u => u.ZipCode.Number, user.ZipCode.Number)
                .Set(u => u.Supplier.Endereco, user.Supplier.Endereco)
                .Set(u => u.Supplier.Nome, user.Supplier.Nome)
                .Set(u => u.Supplier.SupplierId, user.Supplier.SupplierId)
                .Set(u => u.Supplier.Telephone, user.Supplier.Telephone)
                .Set(u => u.Supplier.Cnpj, user.Supplier.Cnpj)
                .Set(u => u.Supplier.Email, user.Supplier.Email);

            var updateResult2 = _usersCollection.ReplaceOne(filter, user);

            // Execute a atualização apenas se o usuário existir
            var updateResult = await _usersCollection.UpdateOneAsync(filter, updateDefinition);

            if (updateResult.ModifiedCount == 0)
            {
                throw new HttpStatusExceptionCustom(StatusCodeEnum.NoContent, "Não houve alteraçào no usuário.");
                //throw HttpStatusExceptionCustom.HtttpStatusCodeExceptionCustom(StatusCodeEnum.NoContent);
            }

            return "Update User Success";
        }*/


        public async Task<string> UpdateUserAsync(GerencylFullRegisterView register)
        {
            var user = _mapper.Map<GerencylRegister>(register);

            await _iauthenticationRepository.UpdateNewOrder(user, register.CNPJ);

            return "Update User Success";
        }

        private async Task<bool> VerifyUserAsync(string cnpj, string email)
        {
            var filter = Builders<GerencylRegister>.Filter.Eq(u => u.CNPJ, cnpj);
            var filter2 = Builders<GerencylRegister>.Filter.Eq(u => u.Email, email);
            var userExists = await _usersCollection.Find(filter).AnyAsync();
            var userExists2 = await _usersCollection.Find(filter2).AnyAsync();

            if (userExists || userExists2)
            {
                return true;
            }
            else
                return false;
        }
    }
}