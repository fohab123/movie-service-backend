using System.Net;
using System.Net.Mail;

namespace movie_service_backend.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendVerificationEmailAsync(string email, string token)
        {
            var verificationLink = $"{_config["FrontendUrl"]}/verify-email?token={token}";

            var message = new MailMessage();
            message.To.Add(email);
            message.Subject = "Verify your account";
            message.Body = $"Click the link to verify your email: {verificationLink}";
            message.IsBodyHtml = false;
            message.From = new MailAddress(_config["Smtp:From"]);

            using var smtp = new SmtpClient(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]))
            {
                Credentials = new NetworkCredential(_config["Smtp:User"], _config["Smtp:Pass"]),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
