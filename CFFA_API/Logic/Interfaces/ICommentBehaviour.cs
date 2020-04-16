using CFFA_API.Models.ViewModels;
using CFFA_API.Models.ViewModels.Creational;
using System.Collections.Generic;

namespace CFFA_API.Logic.Interfaces
{
    public interface ICommentBehaviour
    {
        //CreateUpdateCommentViewModel ViewCommentUpdate(long commentId);
        bool Create(CreateUpdateCommentViewModel model);
        bool Update(CreateUpdateCommentViewModel model);
        //List<CommentViewModel> GetChildrens(long commentId, int pageIndex);
        List<CommentViewModel> GetComments(long postId, int pageIndex);
        void UpVote(string userId, long commentId);
        void DownVote(string userId, long commentId);
        bool TheOwnerIs(long commentId, string userId);
        void Disable(long commentId);
        //void DeleteComment(string userId, long commentId);
    }
}
