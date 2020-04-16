using System;
using System.Collections.Generic;

namespace CFFA_API.Models
{
    public class Comment
    {
        public long CommentId { get; set; }
        public bool? NotSoftDeleted { get; set; }
        public long PostId { get; set; }
        public long? ParentId { get; set; }
        public string UserId { get; set; }
        public string BodyText { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
        public virtual ICollection<CommentVoters> Voters { get; set; }
    }
}
