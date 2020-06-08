using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Controllers.Helpers.EmailSender
{
    public interface IEmailSender
    {
        //Task SendEmailAsync(string email, string subject, string message);

        Task SendEmailConfirmationTokenMessage(string receiver, string token);
        Task SendPasswordResetTokenMessage(string receiver, string token);
    }
}
