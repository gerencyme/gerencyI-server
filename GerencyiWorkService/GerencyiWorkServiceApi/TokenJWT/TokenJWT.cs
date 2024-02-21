using System.IdentityModel.Tokens.Jwt;

namespace GerencylApi.TokenJWT
{
    public class TokenJWT
    {
        private JwtSecurityToken token;

        internal TokenJWT(JwtSecurityToken token, bool isRefreshToken = false)
        {
            this.token = token;
            this.IsRefreshToken = isRefreshToken;
        }
        public DateTime ValidTo => token.ValidTo;

        public string Value => new JwtSecurityTokenHandler().WriteToken(this.token);

        public bool IsRefreshToken { get; }
    }
}
