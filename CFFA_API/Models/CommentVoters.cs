namespace CFFA_API.Models
{
    public class CommentVoters
    {
        public long Id { get; set; }
        public bool? PositiveVot { get; set; }
        public long? CommentId { get; set; }
        public string UserId { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
