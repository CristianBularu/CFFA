using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models.ViewModels
{
    public class RefreshTokenViewModel
    {
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }
}
