namespace CFFA_API.Models
{
    public class PostVoters
    {
        public long Id { get; set; }
        public bool? PositiveVot { get; set; }
        public long PostId { get; set; }
        public virtual Post Post { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
