using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CPRM.Services
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CPRM System", _configuration["EmailConfiguration:From"]));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Zoho uses SSL on port 465
                await client.ConnectAsync(
                    _configuration["EmailConfiguration:SmtpServer"],
                    int.Parse(_configuration["EmailConfiguration:Port"]),
                    SecureSocketOptions.SslOnConnect);

                await client.AuthenticateAsync(
                    _configuration["EmailConfiguration:UserName"],
                    _configuration["EmailConfiguration:Password"]);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}