using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models.ViewModels.Creational
{
    public class RequestResetPasswordTokenViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "email")]
        public string Email { get; set; }
    }
}
