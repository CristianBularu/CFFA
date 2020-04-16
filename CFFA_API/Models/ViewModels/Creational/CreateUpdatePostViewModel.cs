using CFFA_API.Models.ViewModels.Creational;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace CFFA_API.Models.ViewModels.Creational
{
    public class CreateUpdatePostViewModel
    {
        [NonSerialized]
        public string Extension;

        [NonSerialized]
        public string UserId;

        [Required(ErrorMessage = "{0} is required in order to submit a post")]
        [Display(Name = "Sketch Id")]
        public long? SketchId { get; set; }

        public long? Id { get; set; }

        [Required(ErrorMessage = "{0} is required in order to submit a post")]
        [StringLength(89, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [StringLength(4000, ErrorMessage = "The {0} must have maximum {1} characters.")]
        public string BodyText { get; set; }

        [Required(ErrorMessage = "{0} is required in order to submit a post")]
        [Display(Name = "File")]
        public IFormFile File { get; set; }
    }
}
