using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models
{
    public class CustomTokens
    {
        public int Id { get; set; }
        public string ConfirmationTokenValue { get; set; }
        public DateTime ConfirmationTokenCreationTime { get; set; }
        public int ConfirmationTokenAttempts { get; set; }

        public string ResetPasswordTokenValue { get; set; }
        public DateTime ResetPasswordCreationTime { get; set; }
        public int ResetPasswordAttempts { get; set; }

        public string RefreshTokenValue { get; set; }
        public DateTime RefreshTokenCreationTime { get; set; }
        public int RefreshTokenAttempts { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
