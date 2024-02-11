using ApiAuthentication.Models;
using ApiAuthentication.Services.Interfaces.InterfacesRepositories;
using ApiAuthentication.Services.Interfaces.InterfacesServices;
using ApiAuthentication.Token;
using ApiAuthentication.Views;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPIs.Token;

namespace ApiAuthentication.Services
{
    public class AuthenticationService : IAuthenticationServicess
    {
        private readonly IMongoCollection<GerencylRegister> _usersCollection;
        private readonly IAuthenticationRepository _iauthenticationRepository;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<GerencylRegister> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly EmailConfirmationService _sendEmaail;

        public AuthenticationService(IMongoDatabase database, UserManager<GerencylRegister> userManager,
            IOptions<JwtSettings> jwtSettings, EmailConfirmationService sendEmaail, IMapper mapper,
            IAuthenticationRepository iauthenticationRepository, ILogger<AuthenticationService> logger)
        {
            _mapper = mapper;
            _logger = logger;
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

                    var newRefreshToken = await GenerateRefreshTokenAsync(usuario.CNPJ);

                    var token = new TokenJWTBuilder()
                        .AddSecurityKey(JwtSecurityKey.Create(_jwtSettings.SecurityKey))
                        .AddSubject(cnpj)
                        .AddIssuer(_jwtSettings.Issuer)
                        .AddAudience(_jwtSettings.Audience)
                        .AddClaim(ClaimTypes.Role, "Comum")
                        .AddExpiry(60)
                        .Builder();

                    var returnLogin = await ReturnUser(cnpj);
                    returnLogin.RefreshToken = newRefreshToken;
                    returnLogin.Token = token.Value;
                    
                    await _iauthenticationRepository.SaveRefreshTokenAsync(usuario.CNPJ, newRefreshToken);

                    return returnLogin;
                }
            }
            throw new UnauthorizedAccessException();
        }

        public async Task<string> GenerateRefreshTokenAsync(string cnpj)
        {

            var randomNumber = new Random();

            var hash = randomNumber.Next().ToString() + cnpj;

            var tokenBuilder = new TokenJWTBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(_jwtSettings.SecurityKey))
                .AddSubject(hash)
                .AddIssuer(_jwtSettings.Issuer)
                .AddAudience(_jwtSettings.Audience)
                .AddClaim(ClaimTypes.Role, "Comum")
                .WithRefreshTokenExpiration(2880);

            var refreshToken = tokenBuilder.Builder(isRefreshToken: true);
            var retorna = refreshToken.Value;
            return retorna;
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            var user = await _iauthenticationRepository.GetUserByRefreshTokenAsync(refreshToken);
            var isValidRefreshToken = ValidateRefreshToken(refreshToken);
            
            if (isValidRefreshToken)
            {
                var newAccessToken = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create(_jwtSettings.SecurityKey))
                    .AddSubject(user.CNPJ)
                    .AddIssuer(_jwtSettings.Issuer)
                    .AddAudience(_jwtSettings.Audience)
                    .AddClaim(ClaimTypes.Role, "Comum")
                    .AddExpiry(60)
                    .Builder();

                return newAccessToken.Value;
            }
            return "Token inválido";
        }

        private bool ValidateRefreshToken(string refreshToken)
        {
            try
            {
                // Decode o token para acessar suas informações
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(refreshToken) as JwtSecurityToken;

                var expiryDate = jsonToken?.ValidTo;

                if (expiryDate.HasValue)
                {
                    var currentTimeUtc = DateTime.Now;
                    var expiryDateUtc = expiryDate.Value.ToUniversalTime();

                    // Adicionar uma janela de tolerância de alguns minutos, se necessário
                    var toleranceWindow = TimeSpan.FromMinutes(5);
                    return expiryDateUtc > currentTimeUtc - toleranceWindow;
                }

                return false;
            }
            catch (SecurityTokenExpiredException)
            {
                // Lidar especificamente com tokens expirados
                return false;
            }
            catch (Exception ex)
            {
                // Logar a exceção para fins de depuração
                _logger.LogError(ex, "Erro ao validar o token de atualização");
                return false;
            }
        }

        public async Task<GerencylFullRegisterView> ReturnUser(string cnpj)
        {
            var recupera = await _iauthenticationRepository.ReturnUser(cnpj);

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

            await _iauthenticationRepository.AdicionarUsuarioAsync(user);

            var retornaToken = await CriarTokenAsync(user.CNPJ, register.Password);

            return retornaToken;
        }

        public async Task<string> UpdateUserAsync(GerencylFullRegisterView register)
        {
            byte[] imagemBytes = Convert.FromBase64String(register.CompanyImg);

            if (!string.IsNullOrWhiteSpace(register.CompanyImg))
            {
                if (!IsPng(imagemBytes) && !IsJpeg(imagemBytes))
                {
                    throw new HttpStatusExceptionCustom(StatusCodeEnum.NotAcceptable, "A imagem deve ser do tipo PNG ou JPEG.");
                }
            }

            var user = _mapper.Map<GerencylRegister>(register);

            await _iauthenticationRepository.UpdateNewOrder(user, register.CNPJ);

            return "Update User Success";
        }

        public async Task<bool> VerifyUserAsync(string cnpj, string email)
        {
            return await _iauthenticationRepository.VerifyUserAsync(cnpj, email);
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

//var confirmationLink = await _sendEmaail.GenerateConfirmRegister(user);

//await _sendEmaail.SendEmailConfirmationAsync(confirmationLink, user.Email);


//var idUsuario = usuario.Id;

// Verifique se o usuário já possui um refresh token
/*var existingRefreshToken = usuario.RefreshToken;

// Valide o Refresh Token existente
if (ValidateRefreshToken(existingRefreshToken))
{
    // Gere um novo access token
    var token = new TokenJWTBuilder()
        .AddSecurityKey(JwtSecurityKey.Create(_jwtSettings.SecurityKey))
        .AddSubject(cnpj)
        .AddIssuer(_jwtSettings.Issuer)
        .AddAudience(_jwtSettings.Audience)
        .AddClaim(ClaimTypes.Role, "Comum")
        .AddExpiry(60)
        .Builder();

    var returnLogin = await ReturnUser(cnpj);

    returnLogin.RefreshToken = existingRefreshToken;
    returnLogin.Token = token.Value;

    return returnLogin;
}
else
{
    //await _iauthenticationRepository.SaveRefreshTokenAsync(usuario.CNPJ, newRefreshToken);
}*/