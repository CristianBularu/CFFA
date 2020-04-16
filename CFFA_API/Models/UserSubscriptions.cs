namespace CFFA_API.Models
{
    public class UserSubscriptions
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string SubscribedToUserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser SubscribedToUser { get; set; }
    }
}
