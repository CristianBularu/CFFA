using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CFFA_API.Models.ViewModels
{
    public class IndexProfileViewModel : ProfileWithPostsViewModel
    {
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<PostAuthorViewModel> FavoritePosts { get; set; } = new List<PostAuthorViewModel>();
        //public List<ProfileViewModel> Subscriptions { get; set; } = new List<ProfileViewModel>();
        public List<SketchViewModel> Sketches { get; set; } = new List<SketchViewModel>();
    }
}
