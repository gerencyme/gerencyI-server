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
            var usuario = await _usersCollection.Find(u =>
                u.CNPJ.Equals(cnpj, StringComparison.OrdinalIgnoreCase) &&
                u.PasswordHash.Equals(senha, StringComparison.Ordinal))
                .FirstOrDefaultAsync();

            if (usuario != null)
            {
                if (!string.IsNullOrEmpty(senha))
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
                .AddSubject(userId)
                .WithRefreshTokenExpiration(2880);

            var refreshToken = tokenBuilder.Builder(isRefreshToken: true);

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

            user.PasswordHash = register.Password;

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
            byte[] imagemBytes = Convert.FromBase64String(register.CompanyImg);
            if (IsPng(imagemBytes))
            {
                throw new HttpStatusExceptionCustom(StatusCodeEnum.NotAcceptable, "A imagem não é do tipo PNG.");
            }
            if(IsJpeg(imagemBytes))
            {
                throw new HttpStatusExceptionCustom(StatusCodeEnum.NotAcceptable, "A imagem não é do tipo JPEG.");
            }
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

        static bool IsPng(byte[] bytes)
        {
            if (bytes.Length < 8)
            {
                return false;
            }

            // Assinatura PNG
            byte[] pngSignature = { 137, 80, 78, 71, 13, 10, 26, 10 };

            for (int i = 0; i < pngSignature.Length; i++)
            {
                if (bytes[i] != pngSignature[i])
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsJpeg(byte[] bytes)
        {
            if (bytes.Length < 2)
            {
                return false;
            }

            // Assinatura JPEG
            byte[] jpegSignature = { 0xFF, 0xD8 };

            for (int i = 0; i < jpegSignature.Length; i++)
            {
                if (bytes[i] != jpegSignature[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}