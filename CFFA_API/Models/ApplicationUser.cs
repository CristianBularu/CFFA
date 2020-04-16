using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CFFA_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool? NotSoftDeleted { get; set; }
        public string FullName { get; set; }
        public bool? Premium { get; set; }
        public string Extension { get; set; }
        public DateTime CreationTime { get; set; }
        public int CustomTokensId { get; set; }
        public virtual CustomTokens CustomTokens { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<UserSubscriptions> Subscriptions { get; set; } = new List<UserSubscriptions>();
        public virtual ICollection<UserFavoritePosts> FavoritePosts { get; set; } = new List<UserFavoritePosts>();
    }
}
