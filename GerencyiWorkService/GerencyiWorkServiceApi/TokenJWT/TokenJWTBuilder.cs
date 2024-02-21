using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GerencylApi.TokenJWT
{
    public class TokenJWTBuilder
    {
        private SecurityKey securityKey = null;
        private string subject = "";
        private string issuer = "";
        private string audience = "";
        private Dictionary<string, string> claims = new Dictionary<string, string>();
        private int expiryInMinutes = 20;

        public TokenJWTBuilder() { } // Adicionando um construtor padrão

        public TokenJWTBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this.securityKey = securityKey;
            return this;
        }

        public TokenJWTBuilder AddSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public TokenJWTBuilder AddIssuer(string issuer)
        {
            this.issuer = issuer;
            return this;
        }

        public TokenJWTBuilder AddAudience(string audience)
        {
            this.audience = audience;
            return this;
        }

        public TokenJWTBuilder AddClaim(string type, string value)
        {
            this.claims.Add(type, value);
            return this;
        }

        public TokenJWTBuilder AddClaims(Dictionary<string, string> additionalClaims)
        {
            this.claims = this.claims.Union(additionalClaims).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return this;
        }

        public TokenJWTBuilder AddExpiry(int expiryInMinutes)
        {
            this.expiryInMinutes = expiryInMinutes;
            return this;
        }

        private void EnsureArguments()
        {
            if (this.securityKey == null)
                throw new ArgumentException("Security Key is required");

            if (string.IsNullOrEmpty(this.subject))
                throw new ArgumentException("Subject is required");

            if (string.IsNullOrEmpty(this.issuer))
                throw new ArgumentException("Issuer is required");

            if (string.IsNullOrEmpty(this.audience))
                throw new ArgumentException("Audience is required");
        }

        public TokenJWT Builder()
        {
            EnsureArguments();

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, this.subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }
            .Union(this.claims.Select(item => new Claim(item.Key, item.Value)));

            DateTime expiration = DateTime.UtcNow.AddMinutes(expiryInMinutes);

            var token = new JwtSecurityToken(
                issuer: this.issuer,
                audience: this.audience,
                claims: claims,
                expires: expiration,
                signingCredentials: new SigningCredentials(this.securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new TokenJWT(token);
        }

        public TokenJWT Builder(bool isRefreshToken = false)
        {
            EnsureArguments();

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, this.subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }
            .Union(this.claims.Select(item => new Claim(item.Key, item.Value)));

            DateTime expiration = isRefreshToken
                ? DateTime.UtcNow.AddMinutes(refreshTokenExpiryInMinutes)
                : DateTime.UtcNow.AddMinutes(expiryInMinutes);

            var token = new JwtSecurityToken(
                issuer: this.issuer,
                audience: this.audience,
                claims: claims,
                expires: expiration,
                signingCredentials: new SigningCredentials(this.securityKey, SecurityAlgorithms.HmacSha256)
            );

            if (isRefreshToken)
            {
                var refreshExpiration = DateTime.UtcNow.AddMinutes(refreshTokenExpiryInMinutes);
                var refreshClaim = new Claim("refresh_token", "true");
                var refreshJwt = new JwtSecurityToken(
                    issuer: this.issuer,
                    audience: this.audience,
                    claims: new[] { refreshClaim },
                    expires: refreshExpiration,
                    signingCredentials: new SigningCredentials(this.securityKey, SecurityAlgorithms.HmacSha256)
                );
                ((List<Claim>)token.Claims).AddRange(refreshJwt.Claims);
            }

            return new TokenJWT(token, isRefreshToken);
        }

        private int refreshTokenExpiryInMinutes = 1440; // valor padrão, ajuste conforme necessário

        public TokenJWTBuilder WithRefreshTokenExpiration(int minutes)
        {
            this.refreshTokenExpiryInMinutes = minutes;
            return this;
        }

        public TokenJWTBuilder WithExpiration(int minutes)
        {
            this.expiryInMinutes = minutes;
            return this;
        }
    }
}
