using MimeKit;
using MailKit.Net.Smtp;

namespace StoreManagementSystem.API.Helpers
{
    public static class EmailSender
    {
        public static async Task SendEmailAsync(IConfiguration configuration, string email, string subject, string message)
        {
            var fromEmail = configuration["EmailSettings:FromEmail"];
            var password = configuration["EmailSettings:Password"];

            if (string.IsNullOrEmpty(password)) throw new Exception("Email password missing.");

            password = password.Replace(" ", "");

            if (password == "test") return; // Bypass for unit tests

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

