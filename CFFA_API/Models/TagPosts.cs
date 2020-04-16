namespace CFFA_API.Models
{
    public class TagPosts
    {
        public long Id { get; set; }
        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
