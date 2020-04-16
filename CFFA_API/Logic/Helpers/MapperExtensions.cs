using CFFA_API.Models;
using CFFA_API.Models.ViewModels;
using CFFA_API.Models.ViewModels.Creational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Logic.Helpers
{
    public static class MapperExtensions
    {
        public static Sketch AsSketch(this CreateUpdateSketchViewModel viewModel)
        {
            var sketch = new Sketch
            {
                Id = viewModel.Id ?? -1,
                UserId = viewModel.UserId,
                Title = viewModel.Title,
                Extension = viewModel.Extension,
                PageCount = viewModel.PageCount??0,
                PageHeight = viewModel.PageHeight??0
            };
            return sketch;
        }

        public static CreateUpdateSketchViewModel AsCreateUpdateSketchViewModel(this Sketch sketch)
        {
            var viewModel = new CreateUpdateSketchViewModel
            {
                Id = sketch.Id,
                UserId = sketch.UserId,
                Title = sketch.Title,
                Extension = sketch.Extension,
                PageCount = sketch.PageCount,
                PageHeight = sketch.PageHeight
            };
            return viewModel;
        }

        public static Sketch AsSketch(this SketchViewModel viewModel)
        {
            var sketch = new Sketch
            {
                Id = viewModel.Id,
                UserId = viewModel.UserId,
                Title = viewModel.Title,
                Extension = viewModel.Extension,
                PageCount = viewModel.PageCount,
                PageHeight = viewModel.PageHeight
                
            };
            return sketch;
        }

        public static SketchViewModel AsSketchViewModel(this Sketch sketch)
        {
            var viewModel = new SketchViewModel
            {
                Id = sketch.Id,
                UserId = sketch.UserId,
                Title = sketch.Title,
                Extension = sketch.Extension,
                PageCount = sketch.PageCount,
                PageHeight = sketch.PageHeight
            };
            return viewModel;
        }

        #region PostMappers
        public static Post AsPost(this CreateUpdatePostViewModel viewModel)
        {
            var post = new Post
            {
                Id = viewModel.Id ?? -1,
                Title = viewModel.Title,
                UserId = viewModel.UserId,
                Extension = viewModel.Extension,
                BodyText = viewModel.BodyText,
                NotSoftDeleted = true,
                SketchId = viewModel.SketchId ?? 0
                //User = viewModel.User,
            };
            //post.CreationTime will be set in controller
            //post.File will be extracted in controller
            //Voters will eb set in controller
            return post;
        }

        public static Post AsPost(this PostOnlyViewModel viewModel)
        {
            var post = new Post
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Extension = viewModel.Extension
            };
            return post;
        }

        public static Post AsPost(this PostAuthorViewModel viewModel)
        {
            var post = (viewModel as PostOnlyViewModel).AsPost();
            post.User = viewModel.User.AsApplicationUser();
            return post;
        }

        public static Post AsPost(this PostFullWithCommentsViewModel viewModel)
        {
            var post = (viewModel as PostFullViewModel).AsPost();

            post.Comments = new List<Comment>();
            foreach ( CommentViewModel commentViewModel in viewModel.Comments)
            {
                post.Comments.Add(commentViewModel.AsComment());
            }
            //viewModel.ViewerId will be set in controller
            //viewModel.UpVotes/DownVotes will be set in controller
            //viewModel.Favorite will be set in controller
            return post;
        }

        public static Post AsPost(this PostFullViewModel viewModel)
        {
            var post = (viewModel as PostAuthorViewModel).AsPost();
            post.BodyText = viewModel.BodyText;
            //post.SketchId = viewModel.SketchId;
            //post.CreationTime = viewModel.CreationTime;
            return post;
        }


        public static CreateUpdatePostViewModel AsCreateUpdatePostViewModel(this Post post)
        {
            var viewModel = new CreateUpdatePostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                BodyText = post.BodyText,
                SketchId = post.SketchId
            };
            //ViewModel.UserId 
            //viewModel.File should be extracted in controller
            return viewModel;
        }

        public static PostOnlyViewModel AsPostOnlyViewModel(this Post post)
        {
            return post.AsPostFullViewModel();
        }
        public static PostAuthorViewModel AsPostAuthorViewModel(this Post post)
        {
            return post.AsPostFullViewModel();
        }

        public static PostFullViewModel AsPostFullViewModel(this Post post)
        {
            var timeString = post.CreationTime.ToString("yyyy'-'MM'-'dd HH':'mm':'ss'Z'");
            var viewModel = new PostFullWithCommentsViewModel
            {
                Id = post.Id,
                Title = post.Title,
                CreationTime = timeString,
                Extension = post.Extension,
                BodyText = post.BodyText,
                //SketchId = post.SketchId
            };
            return viewModel;
        }

        public static PostFullWithCommentsViewModel AsPostFullWithCommentsViewModel(this Post post)
        {
            var viewModel = new PostFullWithCommentsViewModel
            {
                Id = post.Id,
                Title = post.Title,
                CreationTime = post.CreationTime.ToString("yyyy'-'MM'-'dd HH':'mm':'ss'Z'"),
                Extension = post.Extension,
                BodyText = post.BodyText,
                //SketchId = post.SketchId
            };
            //foreach(Comment comment in post.Comments)
            //{
            //    viewModel.Comments.Add(comment.AsCommentViewModel());
            //}


            //viewModel.UpVotes/DownVOtes will be set in controller
            //viewModel.ViewerId will be set in controller
            //viewModel.Favorite will be set in controller
            return viewModel;
        }

#endregion
        #region ApplicationUserMappers
        public static ApplicationUser AsApplicationUser(this ProfileViewModel viewModel)
        {
            var user = new ApplicationUser
            {
                Id = viewModel.Id,
                FullName = viewModel.FullName,
                Extension = viewModel.Extension
            };
            return user;
        }

        public static ApplicationUser AsApplicationUser(this ProfileWithPostsViewModel viewModel)
        {
            var user = (viewModel as ProfileViewModel).AsApplicationUser();
            user.Posts = new List<Post>();
            foreach(PostOnlyViewModel postViewModel in viewModel.Posts)
            {
                user.Posts.Add(postViewModel.AsPost());
            }
            return user;
        }

        public static ApplicationUser AsApplicationUser(this IndexProfileViewModel viewModel)
        {
            var user = (viewModel as ProfileWithPostsViewModel).AsApplicationUser();
            user.UserName = viewModel.Username;
            user.Email = viewModel.Email;

            //viewModel.FavoritePosts;
            //viewModel.Subscriptions;
            //Should be parsed in controller
            return user;
        }

        public static ProfileViewModel AsProfileViewModel(this ApplicationUser user)
        {
            return user.AsIndexProfileViewModel();
        }

        public static ProfileWithPostsViewModel AsProfileWithPostsViewModel(this ApplicationUser user)
        {
            return user.AsIndexProfileViewModel();
        }

        public static IndexProfileViewModel AsIndexProfileViewModel(this ApplicationUser user)
        {
            var viewModel = new IndexProfileViewModel
            {
                Id = user.Id,
                Extension = user.Extension,
                FullName = user.FullName,
                Email = user.Email
            };
            //viewModel.FavoritePosts;
            //viewModel.Subscriptions;
            //Should be parsed in controller
            return viewModel;
        }
#endregion
        #region Comment Mappers
        public static Comment AsComment(this CommentViewModel viewModel)
        {
            var comment = new Comment
            {
                CommentId = viewModel.CommentId,
                BodyText = viewModel.BodyText,
                //CreationTime = DateTime.Parse(viewModel.CreationTime),
                User = viewModel.User.AsApplicationUser(),
                ParentId = viewModel.ParentId
            };
            //comments will be set in controller
            //voters will be set in controller
            //parent will be set in controller
            //notSoftDeleted will be set in controller
            //Post will be set in controller
            return comment;
        }

        public static Comment AsComment(this CreateUpdateCommentViewModel viewModel)
        {
            var comment = new Comment
            {
                UserId = viewModel.UserId,
                CommentId = viewModel.CommentId ?? 0,
                BodyText = viewModel.BodyText,
                //CreationTime = viewModel.CreationTime,
                PostId = viewModel.PostId ?? 0,
                ParentId = viewModel.ParentId
            };
            return comment;
        }

        public static CommentViewModel AsCommentViewModel(this Comment comment)
        {
            var viewModel = new CommentViewModel
            {
                CommentId = comment.CommentId,
                CreationTime = comment.CreationTime.ToString("yyyy'-'MM'-'dd HH':'mm':'ss'Z'"),
                BodyText = comment.BodyText,
                ParentId = comment.ParentId
            };

            //foreach(Comment subComment in comment.Replies)
            //{
            //    viewModel.Replies.Add(subComment.AsCommentViewModel());
            //}
            //viewModel.UpVotes/DownVotes will be set in controller
            //viewModel.ViewerId will be set in controller
            return viewModel;
        }

        public static CreateUpdateCommentViewModel AsCreateUpdateCommentViewModel(this Comment comment)
        {
            var viewModel = new CreateUpdateCommentViewModel
            {
                UserId = comment.UserId,
                CommentId = comment.CommentId,
                BodyText = comment.BodyText,
                CreationTime = comment.CreationTime.ToString("yyyy'-'MM'-'dd HH':'mm':'ss'Z'"),
                PostId = comment.PostId,
                ParentId = comment.ParentId
            };
            return viewModel;
        }
        #endregion
    }
}
