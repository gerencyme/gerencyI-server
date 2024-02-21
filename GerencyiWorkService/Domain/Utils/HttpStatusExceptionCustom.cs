namespace Domain.Utils.HttpStatusExceptionCustom
{
    public class HttpStatusExceptionCustom : Exception
    {
        public int StatusCode { get; private set; }
        public string Message { get; private set; }

        public HttpStatusExceptionCustom(StatusCodeEnum statusCode, string message) : base(message)
        {
            StatusCode = (int)statusCode;
            Message = message;
        }

        private static readonly Dictionary<StatusCodeEnum, string> StatusCodeMessages = new Dictionary<StatusCodeEnum, string>
    {
        { StatusCodeEnum.Ok, "Ok, Success - The request was successful." },
        { StatusCodeEnum.BadRequest, "Bad Request - The request could not be understood or was missing necessary information." },
        { StatusCodeEnum.NoContent, "No Content - The request was successful, but there is no content in the response body." },
        { StatusCodeEnum.InternalServerError, "Internal Server Error - An unexpected error occurred on the server." },
        { StatusCodeEnum.NotAcceptable, "NotAcceptable" },
        { StatusCodeEnum.Conflict, "Conflict" },
        // Adicione outros códigos de status e mensagens aqui
    };

        public static HttpStatusExceptionCustom HtttpStatusCodeExceptionGeneric(StatusCodeEnum statusCode)
        {
            string message;

            if (StatusCodeMessages.TryGetValue(statusCode, out message))
            {
                return new HttpStatusExceptionCustom(statusCode, message);
            }
            else
            {
                // Se o código de status não estiver no dicionário, você pode lançar uma exceção genérica ou personalizada aqui.
                // Por exemplo, lançando uma exceção com código de status 500 (InternalServerError) e uma mensagem genérica.
                return new HttpStatusExceptionCustom(StatusCodeEnum.InternalServerError, "Internal Server Error - An unexpected error occurred on the server.");
            }
        }

        private static readonly Dictionary<StatusCodeEnum, string> StatusCodeMessagesCustom = new Dictionary<StatusCodeEnum, string>
    {
        { StatusCodeEnum.Ok, "Ok, Success - The request was successful." },
        { StatusCodeEnum.BadRequest, "Email ou Senha inválidoS" },
        { StatusCodeEnum.NoContent, "No Content - The request was successful, but there is no content in the response body." },
        { StatusCodeEnum.InternalServerError, "Internal Server Error - An unexpected error occurred on the server." },
        { StatusCodeEnum.NotAcceptable, "NotAcceptable" },
        { StatusCodeEnum.Conflict, "Usuário já existe" },
        // Adicione outros códigos de status e mensagens aqui
    };

        public static HttpStatusExceptionCustom HtttpStatusCodeExceptionCustom(StatusCodeEnum statusCode)
        {
            string message;

            if (StatusCodeMessagesCustom.TryGetValue(statusCode, out message))
            {
                return new HttpStatusExceptionCustom(statusCode, message);
            }
            else
            {
                // Se o código de status não estiver no dicionário, você pode lançar uma exceção genérica ou personalizada aqui.
                // Por exemplo, lançando uma exceção com código de status 500 (InternalServerError) e uma mensagem genérica.
                return new HttpStatusExceptionCustom(StatusCodeEnum.InternalServerError, "Internal Server Error - An unexpected error occurred on the server.");
            }
        }
    }

    public enum StatusCodeEnum
    {
        // Informacionais
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102,

        // Sucesso
        Ok = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,
        MultiStatus = 207,
        AlreadyReported = 208,
        IMUsed = 226,

        // Redirecionamento
        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        TemporaryRedirect = 307,
        PermanentRedirect = 308,

        // Erros do Cliente
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        PayloadTooLarge = 413,
        URITooLong = 414,
        UnsupportedMediaType = 415,
        RangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        MisdirectedRequest = 421,
        UnprocessableEntity = 422,
        Locked = 423,
        FailedDependency = 424,
        UpgradeRequired = 426,
        PreconditionRequired = 428,
        TooManyRequests = 429,
        RequestHeaderFieldsTooLarge = 431,
        UnavailableForLegalReasons = 451,

        // Erros do Servidor
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HTTPVersionNotSupported = 505,
        VariantAlsoNegotiates = 506,
        InsufficientStorage = 507,
        LoopDetected = 508,
        NotExtended = 510,
        NetworkAuthenticationRequired = 511
    }
}