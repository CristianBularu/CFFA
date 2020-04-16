using System.Collections.Generic;

namespace CFFA_API.Models.ViewModels
{
    public class ProfileWithPostsViewModel : ProfileViewModel
    {
        public int SubscriptionsCount { get; set; }
        public int SubscribersCount { get; set; }
        public int PostsCount { get; set; }
        public bool Subscribed { get; set; }
        public List<PostOnlyViewModel> Posts { get; set; } = new List<PostOnlyViewModel>();
    }
}
