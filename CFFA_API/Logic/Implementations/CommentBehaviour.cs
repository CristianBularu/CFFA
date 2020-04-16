using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using CFFA_API.Models.ViewModels;
using CFFA_API.Models.ViewModels.Creational;
using CFFA_API.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static CFFA_API.Logic.Helpers.MapperExtensions;

namespace CFFA_API.Logic.Implementations
{
    public class CommentBehaviour : ICommentBehaviour
    {
        private static int commentPageSize = 10;
        private readonly ICommentRepository commentRepository;
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepostiroy;

        //public readonly IMapper _mapper;
        public CommentBehaviour(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepostiroy)
        {
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
            this.postRepostiroy = postRepostiroy;
        }

        public List<CommentViewModel> GetComments(long postId, int pageIndex)
        {
            var comments = commentRepository.GetPost_Comments_Users(postId, pageIndex, commentPageSize).ToList();
            var viewModel = new List<CommentViewModel>();
            foreach (Comment comment in comments)
            {
                var votes = commentRepository.GetVotes(comment.CommentId);
                var profileViewModel = userRepository.GetUser(comment.UserId).AsProfileViewModel();
                var commentViewModel = comment.AsCommentViewModel();
                commentViewModel.User = profileViewModel;
                commentViewModel.DownVotes = votes.Down;
                commentViewModel.UpVotes = votes.Up;
                viewModel.Add(commentViewModel);
            }
            return viewModel;
        }

        public List<CommentViewModel> GetChildrens(long commentId, int pageIndex)
        {
            var comments = commentRepository.GetChildComments(commentId, pageIndex, commentPageSize).ToList();
            var viewModel = new List<CommentViewModel>();
            foreach (Comment comment in comments)
            {
                var profileViewModel = userRepository.GetUser(comment.UserId).AsProfileViewModel();
                var commentViewModel = comment.AsCommentViewModel();
                commentViewModel.User = profileViewModel;
                viewModel.Add(commentViewModel);
            }
            return viewModel;
        }

        public bool Create(CreateUpdateCommentViewModel model)
        {
            try
            {
                var comment = new Comment();
                comment.CreationTime = DateTime.UtcNow;
                comment = model.AsComment();
                comment.CommentId = 0;
                commentRepository.Add(comment);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(CreateUpdateCommentViewModel model)
        {
            if (string.IsNullOrEmpty(model.BodyText))
                return false;
            var comment = model.AsComment();
            comment.CreationTime = DateTime.UtcNow;
            return commentRepository.Update(comment);
        }

        public CreateUpdateCommentViewModel ViewCommentUpdate(long commentId)
        {
            var model = commentRepository.Get(commentId).AsCreateUpdateCommentViewModel();
            return model;
        }

        public void UpVote(string userId, long commentId)
        {
            commentRepository.UpdateVote(userId, commentId, true);
        }

        public void DownVote(string userId, long commentId)
        {
            commentRepository.UpdateVote(userId, commentId, false);
        }

        public bool TheOwnerIs(long commentId, string userId)
        {
            return commentRepository.Get(commentId).UserId == userId;
        }

        public void Disable(long commentId)
        {
            commentRepository.Disable(commentId);
        }


        //public void DeleteComment(string userId, long commentId)
        //{
        //    var comment = _commentRepository.GetComment_User(commentId);
        //    if (userId != comment.UserId)
        //        return;
        //    _commentRepository.CustomRemove(comment.CommentId);
        //}
    }
}
