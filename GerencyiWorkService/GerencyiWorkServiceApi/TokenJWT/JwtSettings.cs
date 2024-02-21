namespace ApiAuthentication.Token
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public int TokenExpirationMinutes { get; set; } = 20; // Configuração padrão de expiração do token em minutos
        public int RefreshTokenExpirationDays { get; set; } = 7; // Configuração padrão de expiração do refresh token em dias
    }
}
