using CFFA_API.Models;
using System.Linq;

namespace CFFA_API.Repository.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IQueryable<Comment> GetPost_Comments_Users(long PostID, int pageIndex, int pageSize);
        IQueryable<Comment> GetChildComments(long ParentID, int pageIndex, int pageSize);
        bool Disable(long CommentID);
        (int Up, int Down) GetVotes(long commentId);
        bool? UserVoted(string userId, long commentId);
        void UpdateVote(string userId, long commentId, bool typeVote);
        Comment GetComment_User(long commentId);
        //void CustomRemove(long commentId);
    }
}
