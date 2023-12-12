using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text;
using ApiAuthentication.Models;

namespace ApiAuthentication.Services
{
    public class EmailConfirmationService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<GerencylRegister> _userManager;

        public EmailConfirmationService(
            IConfiguration configuration,
            ISendGridClient sendGridClient,
            IWebHostEnvironment webHostEnvironment,
            UserManager<GerencylRegister> userManager)
        {
            _configuration = configuration;
            _sendGridClient = sendGridClient;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        private string GenerateConfirmationLink( string cnpj, string base64Code)
        {
            var scheme = _webHostEnvironment.IsDevelopment() ? "http" : "https";
            var host = _webHostEnvironment.IsDevelopment() ? "localhost:5000" : "seusite.com";

            return $"{scheme}://{host}/api/ConfirmarEmail?userCNPJ={cnpj}&code={base64Code}";
        }

        private string EncodeCode(string code)
        {
            var encodedCode = Encoding.UTF8.GetBytes(code);
            return WebEncoders.Base64UrlEncode(encodedCode);
        }

        public async Task<string> GenerateConfirmRegister(GerencylRegister user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var base64Code = EncodeCode(code);
            var confirmationLink = GenerateConfirmationLink(user.CNPJ, base64Code);
            return confirmationLink;
        }


        public async Task<string> SendEmailConfirmationAsync(string confirmationLink, string email)
        {
            try
            {

                string fromEmail = _configuration.GetSection("SendGridEmailSettings")
                    .GetValue<string>("FromEmail");

                string fromName = _configuration.GetSection("SendGridEmailSettings")
                    .GetValue<string>("FromName");

                var plainTextContent = $"Obrigado por escolher nossos serviços! Por favor, confirme seu e-mail clicando no link abaixo:\n\n{confirmationLink}";

                var htmlContent = $@"<!DOCTYPE html><html><body><p>Obrigado por escolher nossos serviços! Por favor, confirme seu e-mail clicando no link abaixo:</p><br/>
                                <a href=""{confirmationLink}"">Confirmar E-mail</a></body></html>";

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(fromEmail, fromName),
                    Subject = "Gerencyi - Confirmação de E-mail",
                    PlainTextContent = plainTextContent,
                    HtmlContent = htmlContent
                };

                msg.AddTo(email);
                var response = await _sendGridClient.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    return "E-mail enviado com sucesso";
                }
                else
                {
                    return $"Falha ao enviar o e-mail. Status Code: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Erro ao enviar o e-mail: {ex.Message}";
            }
        }


/*        public async Task<string> SendEmailConfirmationAsync(GerencylRegister user)
        {
            try
            {
                var SendUser = await _userManager.GetUserNameAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var base64Code = EncodeCode(code);

                string fromEmail = _configuration.GetSection("SendGridEmailSettings")
                    .GetValue<string>("FromEmail");

                string fromName = _configuration.GetSection("SendGridEmailSettings")
                    .GetValue<string>("FromName");

                var confirmationLink = GenerateConfirmationLink(user, base64Code);

                var plainTextContent = $"Obrigado por escolher nossos serviços! Por favor, confirme seu e-mail clicando no link abaixo:\n\n{confirmationLink}";

                var htmlContent = $@"<!DOCTYPE html><html><body><p>Obrigado por escolher nossos serviços! Por favor, confirme seu e-mail clicando no link abaixo:</p><br/>
                                <a href=""{confirmationLink}"">Confirmar E-mail</a></body></html>";

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(fromEmail, fromName),
                    Subject = "Gerencyi - Confirmação de E-mail",
                    PlainTextContent = plainTextContent,
                    HtmlContent = htmlContent
                };
                var email = user;
                msg.AddTo(SendUser);
                var response = await _sendGridClient.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    return "E-mail enviado com sucesso";
                }
                else
                {
                    return $"Falha ao enviar o e-mail. Status Code: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Erro ao enviar o e-mail: {ex.Message}";
            }
        }*/



/*
        private string GenerateConfirmationLink(GerencylRegister user, string base64Code)
        {
            var scheme = _webHostEnvironment.IsDevelopment() ? "http" : "https";
            var host = _webHostEnvironment.IsDevelopment() ? "localhost:5000" : "seusite.com";

            return $"{scheme}://{host}/api/ConfirmarEmail?userId={user.CNPJ}&code={base64Code}";
        }*/
    }
}
