using System;
using System.Collections.Generic;

namespace CFFA_API.Models.ViewModels
{
    public class CommentViewModel
    {
        public long? ParentId { get; set; }
        public long CommentId { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public string BodyText { get; set; }
        public virtual string CreationTime { get; set; }
        public virtual List<CommentViewModel> Replies { get; set; }
        public ProfileViewModel User { get; set; }
        public string ViewerId { get; set; }
        public bool? ViewerVoted { get; set; }
    }
}
