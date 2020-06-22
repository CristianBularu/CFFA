using CFFA_API.Models;
using CFFA_API.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CFFA_API.Logic.Implementations.UserBehaviour;

namespace CFFA_API.Logic.Interfaces
{
    public interface IUserBehaviour
    {
        //List<ProfileViewModel> GetSubsByUserId(string userId, int pageIndex, int pageSize);
        ProfileWithPostsViewModel GetVisitProfile(string userId, string currentUserId, int page);
        IndexProfileViewModel GetOwnProfile(string userId, int pageOwnPosts, int pageFavoritePosts, int pageSubscriptions, int pageSketches);
        void ChangeFullName(ApplicationUser user, string model);
        bool IsUserSubscribedTo(string id, string curentUser);
        void SubscribeTo(string curentUserId, string userId);
        void UnSubscribeTo(string curentUserId, string userId);
        bool Disable(string userId);
        CustomTokenState VerifyCustomToken(string probe, string userId, TokenType type);
        CustomTokens GenerateCustomToken(string userId, TokenType type, CustomTokensDecorator additional = null);
        void ClearTokenNoUpdate(CustomTokens tokens);
        void GenerateConfirmationTokenNoUpdate(CustomTokens tokens);
        bool ChangeProfilePhotoPath(ApplicationUser user, string ext);
        Task<List<ProfileViewModel>> Search(string like, int page);
    }
}
