using System.Net.Mail;
using System.Net;
using TaskManager.Application.Common;
using TaskManager.Application.Contracts;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Application.Services {
    public class EmailService : IEmailService {
        private readonly IConfiguration _smtpConfig;

        public EmailService(IConfiguration smtpConfig) { 
            _smtpConfig = smtpConfig;
        }

        public async Task<Result> ConfirmEmail(Guid userId, string email) {
            await SendEmailAsync(email, "Confirm email");

            return Result.Success();
        }

        public async Task<Result> SendEmailAsync(string email, string message, string subject = "") {
            try {
                var smtpClient = new SmtpClient("smtp.example.com") {
                    Port = 587,
                    Credentials = new NetworkCredential("test-test@gmail.com", "your-password"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage {
                    From = new MailAddress("your-email@example.com"),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);

                return Result.Success();
            }
            catch (SmtpException ex) {
                return Result.Success();
            }
            catch (Exception ex) {
                return Result.Success();
            }
        }
    }
}
