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
            message.IsBodyHtml = true;
            message.Body = $@"
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background:#f6f6f6;padding:40px 0;font-family:Arial,sans-serif;"">
              <tr>
                <td align=""center"">
                  <table width=""500"" cellspacing=""0"" cellpadding=""0"" style=""background:white;border-radius:10px;padding:30px;"">
                    <tr>
                      <td align=""center"" style=""font-size:24px;font-weight:bold;color:#333;"">
                        👋 Welcome to MovieApp!
                      </td>
                    </tr>

                    <tr>
                      <td style=""padding-top:20px;font-size:16px;color:#555;line-height:22px;"">
                        Thank you for registering. To activate your account, please confirm your email address by clicking the button below:
                      </td>
                    </tr>

                    <tr>
                      <td align=""center"" style=""padding:30px 0;"">
                        <a href=""{verificationLink}"" 
                           style=""background:#007bff;color:white;text-decoration:none;
                                  padding:14px 28px;border-radius:6px;font-size:16px;display:inline-block;"">
                          Verify Email
                        </a>
                      </td>
                    </tr>

                    <tr>
                      <td style=""font-size:14px;color:#777;padding-top:15px;"">
                        If the button doesn't work, copy and paste the following link:
                      </td>
                    </tr>

                    <tr>
                      <td style=""padding-top:10px;font-size:13px;color:#007bff;word-break:break-all;"">
                        {verificationLink}
                      </td>
                    </tr>

                    <tr>
                      <td style=""padding-top:25px;font-size:12px;color:#aaa;text-align:center;"">
                        © {DateTime.UtcNow.Year} MovieApp Inc. All rights reserved.
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>";
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
