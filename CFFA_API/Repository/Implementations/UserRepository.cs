using CFFA_API.Models;
using CFFA_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static CFFA_API.Logic.Helpers.LinqPaginationExtension;

namespace CFFA_API.Repository.Implementations
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }
        public ApplicationDbContext AppContext { get { return Context as ApplicationDbContext; } }
        public ApplicationUser GetUser(string Id)
        {
            return Context.Set<ApplicationUser>().Where(u => u.Id.Equals(Id)).AsNoTracking().FirstOrDefault();
        }

        public IQueryable<ApplicationUser> GetSubscriptions(string UserID, int pageIndex, int pageSize)
        {
            var subIds = Context.Set<UserSubscriptions>().Where(us => us.UserId == UserID).Select(us => us.SubscribedToUserId);
            var result = Context.Set<ApplicationUser>().Where(u => subIds.Contains(u.Id)).Page(pageIndex, pageSize).AsNoTracking();
            return result;
        }

       

        public bool IsUserSubscribedTo(string id, string curentUser)
        {
            return Context.Set<UserSubscriptions>().Where(u => u.User.Id.Equals(curentUser)).Select(u => u.SubscribedToUser).Where(u => u.Id.Equals(id)).FirstOrDefault() != null;
        }

        public override bool Update(ApplicationUser user)
        {
            var result = GetUser(user.Id);
            if (result == null)
                return false;
            if (!user.NotSoftDeleted??false)
            {
                Disable(user.Id);
                return false;
            }
            result.Premium = user.Premium;
            result.FullName = user.FullName;
            bool sucsess = Context.SaveChanges() > 0;
            return sucsess;
        }

        public bool Disable(string UserID)
        {
            var user = GetUser(UserID);
            if (user == null)
                return false;
            DisableComments(UserID);
            DisablePosts(UserID);
            GetUser(UserID).NotSoftDeleted = false;
            return Context.SaveChanges() > 0;
        }

        public void DisableComments(string UserID)
        {
            var itrtr = Context.Set<Comment>().Where(c => c.UserId.Equals(UserID)).ToList();
            foreach (var item in itrtr)
            {
                item.NotSoftDeleted = false;
            }
            Context.SaveChanges();
        }

        public void DisablePosts(string UserID)
        {
            var itrtr = Context.Set<Post>().Where(c => c.UserId.Equals(UserID)).ToList();
            foreach (var item in itrtr)
            {
                item.NotSoftDeleted = false;
            }
            Context.SaveChanges();
        }

        public bool ChangeName(ApplicationUser user, string fullName)
        {
            if (user == null)
                return false;
            user.FullName = fullName;
            Context.Entry(user).State = EntityState.Modified;
            //Context.Update(user);
            return Context.SaveChanges() > 0;
        }

        public bool ChangeProfileIconPath(ApplicationUser user, string ext)
        {
            if (user == null)
                return false;
            user.Extension = ext;
            Context.Entry(user).State = EntityState.Modified;
            //Context.Update(user);
            return Context.SaveChanges() > 0;
        }

        public bool AddSubscription(string curentUserId, string userId)
        {
            var subscription = new UserSubscriptions { UserId = curentUserId, SubscribedToUserId = userId };
            Context.Set<UserSubscriptions>().Add(subscription);
            Context.Entry(subscription).State = EntityState.Added;
            return Context.SaveChanges() > 0;
        }

        public bool RemoveSubscription(string curentUserId, string userId)
        {
            var subscription = Context.Set<UserSubscriptions>().Where(s => s.UserId.Equals(curentUserId) && s.SubscribedToUserId.Equals(userId)).FirstOrDefault();
            if (subscription == null)
            {
                return false;
            }
            Context.Set<UserSubscriptions>().Remove(subscription);
            Context.Entry(subscription).State = EntityState.Deleted;
            return Context.SaveChanges() > 0;
        }

        public CustomTokens GetGenerateUserCustomTokens(string userId)
        {
            var customToken = Context.Set<CustomTokens>().Where(t => t.UserId.Equals(userId)).AsNoTracking().FirstOrDefault();
            if (customToken == null)
            {
                customToken = new CustomTokens()
                {
                    UserId = userId,
                    ConfirmationTokenAttempts = 0,
                    ConfirmationTokenValue = "Empty",
                    ResetPasswordAttempts = 0,
                    ResetPasswordTokenValue = "Empty",
                    RefreshTokenAttempts = 0,
                    RefreshTokenValue = "Empty"
                };
                Context.Set<CustomTokens>().Add(customToken);
                Context.SaveChanges();
            }
            return customToken;
        }

        public bool UpdateUserCustomTokens(CustomTokens _customTokens)
        {

            Context.Set<CustomTokens>().Update(_customTokens).State = EntityState.Modified;
            return Context.SaveChanges() > 0;
        }

        public int GetSubscriptionsCount(string userId)
        {
            return Context.Set<UserSubscriptions>().Where(usu => usu.UserId == userId).Count();
        }

        public int GetSubscribersCount(string userId)
        {
            return Context.Set<UserSubscriptions>().Where(us => us.SubscribedToUserId == userId).Count();
        }

        public IQueryable<ApplicationUser> Search(string like, int pageIndex, int PageSize)
        {
            return Context.Set<ApplicationUser>().Where(u => u.FullName.ToLower().Contains(like.ToLower())).AsNoTracking();
        }
    }
}
