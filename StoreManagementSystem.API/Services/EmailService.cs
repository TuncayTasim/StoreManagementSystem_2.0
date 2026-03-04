using MailKit.Net.Smtp;
using MimeKit;

namespace StoreManagementSystem.API.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "tuncaytasim24@gmail.com";
            var password = _configuration["EmailSettings:Password"];

            if (string.IsNullOrEmpty(password)) throw new Exception("Email password missing.");

            password = password.Replace(" ", "");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Store System", fromEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(fromEmail, password);
                    await client.SendAsync(emailMessage);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
