using ApiGerencyiGateway.TokenSecurityKey;
using ApiGerencyiGateway.TokenSettigns;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ApiGatewayGerencyl.TokenValidationMiddleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _configuration;
        private readonly ITokenCache _tokenCache;
        private readonly ILogger<TokenValidationMiddleware> _logger;

        public TokenValidationMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            ITokenCache tokenCache,
            ILogger<TokenValidationMiddleware> logger,
            JwtSettings jwtSettings)
        {
            _next = next;
            _configuration = configuration;
            _tokenCache = tokenCache;
            _logger = logger;
            _jwtSettings = jwtSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
                context.Request.Path.Value.Equals("/api/GenereateTokenIdentity", StringComparison.OrdinalIgnoreCase))
            {
                // Lógica para lidar com solicitação de login
                // Neste caso, você pode permitir a solicitação passar sem mais verificações
                Console.WriteLine("Request para GenerateTokenIdentity recebida. Permitindo a passagem.");
                Console.WriteLine($"Method: {context.Request.Method}, Path: {context.Request.Path}");
                await _next(context);
                return;
            }

            // Extrair o token de acesso e o refresh token
            var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var refreshToken = context.Request.Headers["RefreshToken"].FirstOrDefault();
            try
            {

                /*if (TokenIsInvalid(accessToken))
                {
                    _tokenCache.AddValidToken(accessToken);
                };*/
                // Verificação do token de acesso no cache
                if (_tokenCache.TryGetValidToken(accessToken, out var isAccessTokenValid) && isAccessTokenValid)
                {
                    _logger.LogInformation("Token de acesso válido encontrado no cache. Permitindo a requisição.");
                    await _next(context);
                    return;
                }

                // Validação do token de acesso
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var isValid = TokenIsInvalid(accessToken);
                    if (!isValid)
                    {
                        _logger.LogInformation("Token de acesso válido. Permitindo a requisição.");
                        // Update the cache with the new tokens
                        _tokenCache.AddValidToken(accessToken);
                        await _next(context);
                        return;
                    }
                }

                // Validação do refresh token
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var authenticationServiceUrl = "http://localhost:5252";// "https://gerencyiauthentication.azurewebsites.net/";//_configuration["AuthenticationServiceUrl"];

                    using (var httpClient = new HttpClient())
                    {
                        var jwtReponse = new JwtTokenResponse();
                        jwtReponse.accessToken = accessToken;
                        jwtReponse.refreshToken = refreshToken;

                        // Serialize the object
                        string jsonContent = System.Text.Json.JsonSerializer.Serialize(jwtReponse);
                        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync($"{authenticationServiceUrl}/api/RefreshToken", stringContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var newTokens = await System.Text.Json.JsonSerializer.DeserializeAsync<JwtTokenResponse>(response.Content.ReadAsStream());

                            // Update the cache with the new tokens
                            _tokenCache.AddValidToken(newTokens.accessToken);
                            _tokenCache.AddValidToken(newTokens.refreshToken);

                            // **Update the original request with the new access token:**
                            context.Request.Headers["Authorization"] = $"bearer {newTokens.accessToken}";
                            context.Response.Headers["Authorization"] = $"bearer {newTokens.accessToken}";

                            // **Proceed with the original request using the updated token:**
                            await _next(context);
                            return;
                        }
                    }
                }

                // Se o token de acesso e o refresh token forem inválidos, negar a requisição
                _logger.LogWarning("Token de acesso e/ou refresh token inválidos. Negando a requisição.");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao processar a requisição.");
                throw;
            }


        }

        private bool TokenIsInvalid(string accessToken)
        {
            // Use uma biblioteca JWT para validar o token
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Configurar a validação do token
                var validationParameters = new TokenValidationParameters
                {
                    // Sua chave de segurança usada para assinar o token
                    IssuerSigningKey = JwtSecurityKey.Create(_jwtSettings.SecurityKey),
                    ValidateLifetime = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience
                };

                // Validação do token
                SecurityToken validatedToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(accessToken, validationParameters, out validatedToken);

                // Adicione qualquer lógica de validação adicional aqui, como verificar claims específicos
                // Exemplo: Se um claim "Role" não estiver presente ou tiver um valor incorreto, considere o token inválido
                var roleClaim = principal.FindFirst(ClaimTypes.Role);
                if (roleClaim == null || roleClaim.Value != "Comum")
                {
                    _logger.LogWarning("Ocorreu um erro ao processar a requisição Claim inválida.");
                    return true;

                }

                // Se chegou até aqui, o token é considerado válido
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ocorreu um erro ao processar a requisição Token inválido."); ;
                return true;
            }
        }
    }

    public interface ITokenCache
    {
        void AddValidToken(string token);
        bool TryGetValidToken(string token, out bool isValid);
    }

    public class InMemoryTokenCache : ITokenCache
    {
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(58);
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public void AddValidToken(string token)
        {
            _cache.Set(token, true, _cacheDuration);
        }

        public bool TryGetValidToken(string token, out bool isValid)
        {
            return _cache.TryGetValue(token, out isValid);
        }
    }
}