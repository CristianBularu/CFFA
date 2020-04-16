using System.ComponentModel.DataAnnotations;

namespace CFFA_API.Models.ViewModels.Creational
{
    public class ChangeFullNameViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(55, ErrorMessage = "The {0} has max {1} characters long.")]
        public string FullName { get; set; }
    }
}
