using CFFA_API.Models;
using System.Linq;

namespace CFFA_API.Repository.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        IQueryable<Post> GetTopVoted(int pageIndex, int pageSize);
        IQueryable<Post> GetUserPosts(string UserID, int pageIndex, int pageSize);
        IQueryable<Post> GetFavoritePosts(string userId, int pageIndex, int pageSize);
        IQueryable<Post> GetNewestPosts(int pageIndex, int pageSize);
        void UpdateVote(string userId, long postId, bool typeVote);
        bool? UserVoted(string userId, long postId);
        Post GetPost(long postId);
        bool Disable(long PostId);
        (int Up, int Down) Votes(long postId);
        int CountCommentsOf(long postId);
        void FavoritePosts(string userId, long postId, bool v);
        public bool HasFavorite(string userId, long postId);
        IQueryable<Post> GetFeed(string userId, int pageIndex, int pageSize);
        IQueryable<Post> Search(string like, int pageIndex, int pageSize);
        bool Exists(long postId);

        Sketch CreateSketch(Sketch sketch);
        Sketch GetSketch(long sketchId);
        IQueryable<Sketch> GetUserSketches(string userId, int pageIndex, int pageSize);
        int GetPostCountOfUser(string userId);
        //IQueryable<Post> GetDiscussions();//Depricated
        //IQueryable<Post> GetFavoritePosts1(string userId, int pageIndex, int pageSize);//?

        //Post GetPostWithUserById(long postId);
        //Post GetFullPost(long postId);
        //Post GetPostWithVotes(long postId);
    }
}
