using System.ComponentModel.DataAnnotations;

namespace CFFA_API.Models.ViewModels
{
    public class PostOnlyViewModel
    {
        [Required]
        [StringLength(244, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string Extension { get; set; }
        public long Id { get; set; }
    }
}
