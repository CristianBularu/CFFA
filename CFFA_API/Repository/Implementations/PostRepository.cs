using CFFA_API.Models;
using CFFA_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static CFFA_API.Logic.Helpers.LinqPaginationExtension;

namespace CFFA_API.Repository.Implementations
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context) { }
        public ApplicationDbContext AppContext { get { return Context as ApplicationDbContext; } }
        public IQueryable<Post> GetFavoritePosts(string userId, int pageIndex, int pageSize)
        {
            var favPostIds = Context.Set<UserFavoritePosts>().Where(fp => fp.UserId == userId).Select(fp => fp.PostId);
            var result = Context.Set<Post>().Where(p => favPostIds.Contains(p.Id)).Page(pageIndex, pageSize).AsNoTracking();
            return result;
            //return Context.Set<UserFavoritePosts>()
            //    .Where(u => u.UserId.Equals(userId))
            //    .Select(fp => fp.Post)
            //    .Include(p => p.User)
            //    .Page(pageIndex, pageSize)
            //    .AsNoTracking();
            
        }
        //public IQueryable<Post> GetFavoritePosts1(string userId, int pageIndex, int pageSize)
        //{
        //    return Context.Set<ApplicationUser>()
        //        .Where(u => u.Id.Equals(userId))
        //        .Include(u => u.FavoritePosts)
        //        .SelectMany(u => u.FavoritePosts
        //            .Select(fp => fp.Post))
        //            .Page(pageIndex, pageSize)
        //            .AsNoTracking();
        //}
        public int CountCommentsOf(long postId)
        {
            return Context.Set<Comment>().Where(c=>c.PostId == postId).Count();
        }
        public bool HasFavorite(string userId, long postId)
        {
            var post = Context.Set<ApplicationUser>()
                .Where(u => u.Id.Equals(userId))
                .Include(u => u.FavoritePosts)
                .SelectMany(u => u.FavoritePosts
                .Select(fp => fp.Post)).Where(p=>p.Id == postId).FirstOrDefault();
            return post != null ? true : false;
        }

        public Post GetPost(long postId)
        {
            return Context.Set<Post>().Where(p => p.Id == postId).AsNoTracking().FirstOrDefault();
        }

        public IQueryable<Post> GetNewestPosts(int pageIndex, int pageSize)
        {
            return Context.Set<Post>().OrderByDescending(p => p.CreationTime).Include(p => p.User).Page(pageIndex, pageSize).AsNoTracking();
        }

        //public IQueryable<Post> GetDiscussions()
        //{
        //    return Context.Set<Post>().Where(p => p.BmpPath == null || p.BmpPath == "").Include(p => p.User).AsNoTracking();
        //}

        public IQueryable<Post> GetTopVoted(int pageIndex, int pageSize)
        {
            //var result = Context.Set<PostVoters>()
            //    .Where(pv => pv.PositiveVot)
            //    .GroupBy(pv => pv.PostId)
            //    .OrderByDescending(gr => gr.Count()).DefaultIfEmpty().Page(pageIndex, pageSize).Select(gr=>gr.Key);
            ////.SelectMany(gr => Context.Set<Post>().Where(p => p.Id == gr.Key)).AsNoTracking().DefaultIfEmpty().Page(pageIndex, pageSize);
            //var final = Context.Set<Post>().Where(p => result.Contains(p.Id)).DefaultIfEmpty();
            ////return result; Context.Set<Post>().Where(p=>p.Id == 0)
            //System.Console.WriteLine("Result========================================");
            //System.Console.WriteLine(result.ToList().Count);
            //return final;

            var postIds = Context.Set<PostVoters>()
                .Where(pv=>pv.PositiveVot??false)
                .GroupBy(pv=>pv.PostId)
                .Select(gr => new { PostId = gr.Key, VotesCount = gr.Count()})
                .OrderByDescending(gr=>gr.VotesCount)
                .Select(gr=>gr.PostId).Page(pageIndex, pageSize);
            var posts = Context.Set<Post>().Where(p => postIds.Contains(p.Id)).AsNoTracking();
            return posts;
        }

        public IQueryable<Post> GetFeed(string userId, int pageIndex, int pageSize)
        {

            var subUserIds = Context.Set<UserSubscriptions>().Where(us => us.UserId == userId).Select(us => us.SubscribedToUserId).ToList();
            var resultSubquery = Context.Set<Post>()
                .Where(p => subUserIds.Contains(p.UserId))
                .OrderBy(p => p.CreationTime)
                .Page(pageIndex, pageSize)
                .AsNoTracking();
            //var result = Context.Set<UserSubscriptions>()
            //    .Where(u1 => u1.UserId == userId).Include(us1 => us1.SubscribedToUser)
            //    .Select(sub => sub.SubscribedToUser).Include(u2 => u2.Posts)
            //    .SelectMany(us2 => us2.Posts)
            //    .OrderBy(p => p.CreationTime)
            //    .Page(pageIndex, pageSize).AsNoTracking()
            //    .DefaultIfEmpty();
            //var result1 = Context.Set<ApplicationUser>()
            //    .Where(u => u.Id == userId).Include(u => u.Subscriptions)
            //    .SelectMany(s => s.Subscriptions).Include(s => s.SubscribedToUser)
            //    .Select(sub => sub.SubscribedToUser).Include(sub => sub.Posts)
            //    .SelectMany(f => f.Posts)
            //    .OrderBy(p => p.CreationTime)
            //    .Page(pageIndex, pageSize)
            //    .DefaultIfEmpty();
            return resultSubquery;
        }

        public IQueryable<Post> Search(string like, int pageIndex, int pageSize)
        {
            return Context.Set<Post>().Where(p => p.Title.Contains(like, System.StringComparison.OrdinalIgnoreCase)).Page(pageIndex, pageSize).AsNoTracking();
        }

        public IQueryable<Post> GetUserPosts(string UserID, int pageIndex, int pageSize)
        {
            return Context.Set<ApplicationUser>().Where(p => p.Id.Equals(UserID))
                .SelectMany(u => u.Posts).Page(pageIndex, pageSize).AsNoTracking();
        }

        public (int Up, int Down) Votes(long postId)
        {
            var positive = Context.Set<PostVoters>().Where(p => p.PostId == postId).Count(p => p.PositiveVot??false);
            var negative = Context.Set<PostVoters>().Where(p => p.PostId == postId).Count(p => !(p.PositiveVot??false));
            return (positive, negative);
        }

        public bool? UserVoted(string userId, long postId)
        {
            var pVotes = Context.Set<PostVoters>().Where(pv => pv.UserId.Equals(userId) && pv.PostId == postId).AsNoTracking().FirstOrDefault();
            if (pVotes == null)
                return null;
            else if (pVotes.PositiveVot??false)
                return true;
            else
                return false;
        }

        public override bool Update(Post post)
        {
            var result = Context.Set<Post>().Where(p => p.Id == post.Id).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            result.Title = post.Title;
            result.BodyText = post.BodyText;
            Context.Entry(result).State = EntityState.Modified;
            return Context.SaveChanges() > 0;
        }

        public bool Exists(long postId)
        {
            return Context.Set<Post>().Where(p => p.Id == postId).FirstOrDefault() != null;
        }

        public bool Disable(long postId)
        {
            var post = Context.Set<Post>().Where(p => p.Id == postId).FirstOrDefault();
            if (post==null)
            {
                return false;
            }
            DisableComments(postId);
            post.NotSoftDeleted = false;
            Context.Entry(post).Property("NotSoftDeleted").IsModified = true;
            return Context.SaveChanges() > 0;
        }

        public void UpdateVote(string userId, long postId, bool typeVote)
        {
            var uniqVote = Context.Set<PostVoters>().Where(pv => pv.PostId == postId && pv.UserId.Equals(userId)).FirstOrDefault();

            if (uniqVote == null)
            {
                uniqVote = new PostVoters { UserId = userId, PostId = postId, PositiveVot = typeVote };
                Context.Set<PostVoters>().Add(uniqVote);
                Context.Entry(uniqVote).State = EntityState.Added;
                Context.SaveChanges();
                return;
            }
            Context.Entry(uniqVote).State = EntityState.Modified;
            uniqVote.PositiveVot = typeVote;
            //Context.Entry(uniqVote).Property("PositiveVot").IsModified = true;
            Context.Update(uniqVote);
            Context.SaveChanges();
        }

        public void FavoritePosts(string userId, long postId, bool v)
        {
            if (v)
            {
                var fp = Context.Set<UserFavoritePosts>().Where(f => f.PostId == postId && f.UserId == userId).FirstOrDefault();
                if (fp != null)
                    return;
                fp = new UserFavoritePosts { UserId = userId, PostId = postId };
                if (fp != null)
                {
                    lock (fp)
                    {
                        if (fp != null)
                        {
                            Context.Set<UserFavoritePosts>().Add(fp);
                            //Context.Entry(fp).State = EntityState.Added;
                            Context.SaveChanges();
                        }
                    }
                    fp = null;
                }
            }
            else
            {
                var fp = Context.Set<UserFavoritePosts>().Where(f => f.PostId == postId && f.UserId.Equals(userId)).FirstOrDefault();
                if (fp != null)
                {
                    lock (fp)
                    {
                        if (fp != null)
                        {
                            Context.Set<UserFavoritePosts>().Remove(fp);
                            Context.Entry(fp).State = EntityState.Deleted;
                            Context.SaveChanges();
                        }
                    }
                    fp = null;
                }
            }
        }

        public Sketch CreateSketch(Sketch sketch)
        {
            sketch.Id = 0;
            Context.Set<Sketch>().Add(sketch);
            Context.SaveChanges();
            return sketch;
        }

        public Sketch GetSketch(long sketchId)
        {
            return Context.Set<Sketch>().Where(s => s.Id == sketchId).AsNoTracking().FirstOrDefault();
        }

        public IQueryable<Sketch> GetUserSketches(string userId, int pageIndex, int pageSize)
        {
            return Context.Set<Sketch>().Where(s => s.UserId == userId).Page(pageIndex, pageSize).AsNoTracking();
        }

        private void DisableComments(long PostID)
        {
            var itrtr = Context.Set<Comment>().Where(c => c.PostId == PostID).ToList();
            foreach (var item in itrtr)
            {
                item.NotSoftDeleted = false;
                Context.Entry(item).Property("NotSoftDeleted").IsModified = true;
            }
            Context.SaveChanges();
        }

        public int GetPostCountOfUser(string userId)
        {
            return Context.Set<Post>().Where(p => p.UserId == userId).Count();
        }
    }
}