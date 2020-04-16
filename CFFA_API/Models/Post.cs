using System;
using System.Collections.Generic;

namespace CFFA_API.Models
{
    public class Post
    {
        public long Id { get; set; }
        public bool? NotSoftDeleted { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public string Extension { get; set; }
        public long SketchId { get; set; }
        public DateTime CreationTime { get; set; }
        public virtual Sketch Sketch { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<TagPosts> Tags { get; set; } = new List<TagPosts>();
        public virtual ICollection<PostVoters> Voters { get; set; } = new List<PostVoters>();
    }
}
