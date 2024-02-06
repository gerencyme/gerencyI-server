using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiGerencyiGateway.TokenSecurityKey
{
    public class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}
