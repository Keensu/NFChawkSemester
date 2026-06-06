using NFChawk.Models.Interfaces;
using NFChawk.Models.ViewModels;
using System.Net;
using System.Net.Mail;

namespace NFChawk.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceViewModel _emailConfig;

        public EmailService(EmailServiceViewModel emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port)
            {
                Credentials = new NetworkCredential(
                    _emailConfig.Username,
                    _emailConfig.Password),

                EnableSsl = true
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(
                    _emailConfig.SenderEmail,
                    _emailConfig.SenderName),

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}