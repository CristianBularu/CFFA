using CFFA_API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Repository.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        void DisablePosts(string UserID);
        void DisableComments(string UserID);
        bool Disable(string UserID);
        bool ChangeName(ApplicationUser user, string fullName);
        bool ChangeProfileIconPath(ApplicationUser user, string ext);
        ApplicationUser GetUser(string Id);
        IQueryable<ApplicationUser> GetSubscriptions(string UserID, int pageIndex, int pageSize);
        //ApplicationUser GetUserWithPersonalPosts(string Id);
        bool AddSubscription(string curentUserId, string userId);
        bool RemoveSubscription(string curentUserId, string userId);
        bool IsUserSubscribedTo(string id, string curentUser);
        CustomTokens GetGenerateUserCustomTokens(string userId);
        //CustomTokens GenerateCustomTokens(string userId, bool typeOneToken);
        bool UpdateUserCustomTokens(CustomTokens customTokens);
        int GetSubscriptionsCount(string userId);
        int GetSubscribersCount(string userId);

        IQueryable<ApplicationUser> Search(string like, int pageIndex, int PageSize);
    }
}
