using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CFFA_API.Controllers.Helpers.EmailSender
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            this.logger = logger;
        }

        public async Task SendEmailConfirmationTokenMessage(string receiver, string token)
        {
            MailMessage message = new MailMessage(from: new MailAddress(AppSettings.EmailSenderAddress), new MailAddress(receiver))
            {
                Subject = "Confirmation token from CutFoldFA",
                Body = "Your email address was used to register to the platform CutFoldFA.\nIf you want to confirm registration, please use this token:<font size=\"6\">" + token + "</font> to validate.\nIf you don't recognize this action, please ignore this message."
            };
            await SendMessage(message);
        }

        public async Task SendPasswordResetTokenMessage(string receiver, string token)
        {
            MailMessage message = new MailMessage(from: new MailAddress(AppSettings.EmailSenderAddress), new MailAddress(receiver))
            {
                Subject = "Password reset token from CutFoldFA",
                Body = "Password reset was initiated on your account.\nIf you want to reset the password, please use this token: " + token + " to reset the password.\nIf you don't recognize this action, please ignore this message."
            };
            await SendMessage(message);
        }

        private async Task SendMessage(MailMessage message)
        {
            try { 
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = AppSettings.EmailSenderAddress,
                    Password = AppSettings.EmailSenderPassword
                };
                smtpClient.EnableSsl = true;

                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                //smtpClient.Send(message);
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
            }
        }
    }
}
