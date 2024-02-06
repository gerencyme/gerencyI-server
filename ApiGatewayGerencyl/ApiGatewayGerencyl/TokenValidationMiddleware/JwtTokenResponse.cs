namespace ApiGatewayGerencyl.TokenValidationMiddleware
{
    [Serializable]
    public class JwtTokenResponse
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}
