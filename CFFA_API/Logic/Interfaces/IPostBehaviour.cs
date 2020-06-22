using CFFA_API.Models;
using CFFA_API.Models.ViewModels;
using CFFA_API.Models.ViewModels.Creational;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFFA_API.Logic.Interfaces
{
    
    public interface IPostBehaviour
    {
        delegate Task<bool> PostDecorator(Post User);
        long Create(CreateUpdatePostViewModel model);
        bool Update(CreateUpdatePostViewModel model);
        SketchViewModel CreateSketch(Sketch sketch);
        string TryCopyOfSketch(long sketchId, int leafs, float height);
        //bool RemoveSketch(Sketch sketch);
        //(int Up, int Down) Votes(long postId);
        //bool HasFavorite(string userId, long postId);
        bool TheOwnerIs(long postId, string userId);
        bool UpVote(string userId, long postId);
        bool DownVote(string userId, long postId);
        void FavoritePost(string userId, long postId);
        void UnFavoritePost(string userId, long postId);
        void Disable(long postId);
        //CreateUpdatePostViewModel GetUpdatePostView(long postId);
        //PostFullViewModel GetPostFull(long postId, ApplicationUser viewer);
        PostFullWithCommentsViewModel GetPostFullWithComments(long postId, int pageIndex, string userId);
        //List<PostOnlyViewModel> GetPostsOnlyByUserId(string userId, int pageIndex, int pageSize);
        //List<PostAuthorViewModel> GetFavoritePostsByUserId(string userId, int pageIndex, int pageSize);
        HomeThreadsViewModel GetPostsByThreads(string userId);
        List<PostFullViewModel> GetPopularPosts(int pageIndex, ApplicationUser viewer);
        List<PostFullViewModel> GetFreshPosts(int pageIndex, ApplicationUser viewer);
        List<PostFullViewModel> GetFeedPosts(ApplicationUser user, int pageIndex);
        bool PostExists(long postId);
        Task<List<PostOnlyViewModel>> Search(string like, int page);
    }
}
