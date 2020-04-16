using System;
using System.ComponentModel.DataAnnotations;

namespace CFFA_API.Models.ViewModels.Creational
{
    public class CreateUpdateCommentViewModel
    {
        [NonSerialized]
        public string UserId;

        [NonSerialized]
        public string CreationTime;

        public long? CommentId { get; set; }

        [Required(ErrorMessage = "All comments are part of a Post")]
        public long? PostId { get; set; }

        public long? ParentId { get; set; }

        [Required(ErrorMessage = "Body text is required")]
        [Display(Name = "Comment")]
        public string BodyText { get; set; }
    }
}
