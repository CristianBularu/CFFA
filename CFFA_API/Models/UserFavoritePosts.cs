namespace CFFA_API.Models
{
    public class UserFavoritePosts
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public long PostId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
    }
}
