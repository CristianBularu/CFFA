using CFFA_API.Models;
using CFFA_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static CFFA_API.Logic.Helpers.LinqPaginationExtension;

namespace CFFA_API.Repository.Implementations
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(DbContext context) : base(context) { }
        public ApplicationDbContext AppContext { get { return Context as ApplicationDbContext; } }
        public IQueryable<Comment> GetPost_Comments_Users(long PostID, int pageIndex, int pageSize)
        {
            return Context.Set<Post>()
                .Where(p => p.Id == PostID)
                .SelectMany(p => p.Comments).Page(pageIndex, pageSize).AsNoTracking();
        }

        public IQueryable<Comment> GetChildComments(long ParentID, int pageIndex, int pageSize)
        {
            return Context.Set<Comment>()
                .Where(c => c.CommentId == ParentID)
                .SelectMany(c => c.Replies).Page(pageIndex, pageSize).AsNoTracking();
        }

        public (int Up, int Down) GetVotes(long commentId)
        {
            var positive = Context.Set<CommentVoters>().Where(p => p.CommentId == commentId).Count(p => p.PositiveVot ?? false);
            var negative = Context.Set<CommentVoters>().Where(p => p.CommentId == commentId).Count(p => !p.PositiveVot ?? false);
            return (positive, negative);
        }

        public bool? UserVoted(string userId, long commentId)
        {
            var pVotes = Context.Set<CommentVoters>().Where(pv => pv.UserId.Equals(userId) && pv.CommentId == commentId).AsNoTracking().FirstOrDefault();
            if (pVotes == null)
                return null;
            else if (pVotes.PositiveVot ?? false)
                return true;
            else
                return false;
        }

        public override bool Update(Comment comment)
        {
            var result = Context.Set<Comment>().Where(c => c.CommentId == comment.CommentId).FirstOrDefault();
            if (result == null)
                return false;
            result.BodyText = comment.BodyText;
            Context.Entry(result).Property("BodyText").IsModified = true;
            return Context.SaveChanges() > 0;
        }

        public void UpdateVote(string userId, long commentId, bool typeVote)
        {
            var uniqVote = Context.Set<CommentVoters>().Where(pv => pv.CommentId == commentId && pv.UserId.Equals(userId)).FirstOrDefault();

            if (uniqVote == null)
            {
                uniqVote = new CommentVoters { UserId = userId, CommentId = commentId, PositiveVot = typeVote };
                Context.Set<CommentVoters>().Add(uniqVote);
                Context.Entry(uniqVote).State = EntityState.Added;
                Context.SaveChanges();
                return;
            }
            if (typeVote != uniqVote.PositiveVot)
            {
                uniqVote.PositiveVot = typeVote;
                Context.Entry(uniqVote).Property("PositiveVot").IsModified = true;
                Context.Update(uniqVote);
                Context.SaveChanges();
            }
        }

        public bool Disable(long commentId)
        {
            var comment = Context.Set<Comment>().Where(c => c.CommentId == commentId).FirstOrDefault();
            if (comment == null)
                return false;
            DisableComments(commentId);
            comment.NotSoftDeleted = false;
            Context.Entry(comment).Property("NotSoftDeleted").IsModified = true;
            return Context.SaveChanges() > 0;
        }

        private void DisableComments(long ParentID)
        {
            var itrtr = Context.Set<Comment>().Where(c => c.ParentId == ParentID).ToList();
            foreach (var item in itrtr)
            {
                item.NotSoftDeleted = false;
                Context.Entry(item).Property("NotSoftDeleted").IsModified = true;
            }
            Context.SaveChanges();
        }

        public Comment GetComment_User(long commentId)
        {
            return Context.Set<Comment>().Where(c => c.CommentId == commentId).AsNoTracking().FirstOrDefault();
        }

        //public void CustomRemove(long commentId)
        //{
        //    var comment = Context.Set<Comment>().Where(c => c.CommentId == commentId).FirstOrDefault();
        //    if (comment != null)
        //    {
        //        lock (comment)
        //        {
        //            if (comment != null)
        //            {
        //                Context.Remove(comment);
        //                Context.Entry(comment).State = EntityState.Deleted;
        //                Context.SaveChanges();
        //            }
        //        }
        //        comment = null;
        //    }
        //}
    }
}
