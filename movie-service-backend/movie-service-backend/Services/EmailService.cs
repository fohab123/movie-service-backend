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

        public async Task SendVerificationEmailAsync(string email, string verificationCode)
        {
            var message = new MailMessage();
            message.To.Add(email);
            message.Subject = "Your verification code";
            message.IsBodyHtml = true;
            message.Body = $@"
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background:#f6f6f6;padding:40px 0;font-family:Arial,sans-serif;"">
              <tr>
                <td align=""center"">
                  <table width=""500"" cellspacing=""0"" cellpadding=""0"" style=""background:white;border-radius:10px;padding:30px;"">
                    <tr>
                      <td align=""center"" style=""font-size:24px;font-weight:bold;color:#333;"">
                        Welcome to MovieApp!
                      </td>
                    </tr>

                    <tr>
                      <td style=""padding-top:20px;font-size:16px;color:#555;line-height:22px;text-align:center;"">
                        Thank you for registering. Use the code below to verify your email address:
                      </td>
                    </tr>

                    <tr>
                      <td align=""center"" style=""padding:30px 0;"">
                        <div style=""font-size:36px;font-weight:bold;letter-spacing:8px;color:#333;
                                    background:#f0f0f0;display:inline-block;padding:16px 32px;border-radius:10px;"">
                          {verificationCode}
                        </div>
                      </td>
                    </tr>

                    <tr>
                      <td style=""font-size:14px;color:#777;text-align:center;"">
                        Enter this code on the verification page to activate your account.
                      </td>
                    </tr>

                    <tr>
                      <td style=""padding-top:25px;font-size:12px;color:#aaa;text-align:center;"">
                        &copy; {DateTime.UtcNow.Year} MovieApp Inc. All rights reserved.
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