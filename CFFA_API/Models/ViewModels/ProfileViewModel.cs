using System.ComponentModel.DataAnnotations;

namespace CFFA_API.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }
        public string Extension { get; set; }
    }
}
