
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CinemaGoProject.Services
{
    internal class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                emailSettings["SenderName"],
                emailSettings["SenderEmail"]));

            email.To.Add(new MailboxAddress("", toEmail));

            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = message
            };

            using var client = new SmtpClient();

            client.Connect(
                emailSettings["Server"],
                int.Parse(emailSettings["Port"]),
                MailKit.Security.SecureSocketOptions.StartTls);

            client.Authenticate(
                emailSettings["SenderEmail"],
                emailSettings["Password"]);

            client.Send(email);
            client.Disconnect(true);
        }
    }
}
