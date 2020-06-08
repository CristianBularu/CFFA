using Algorithm;
using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using CFFA_API.Models.ViewModels;
using CFFA_API.Models.ViewModels.Creational;
using CFFA_API.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static CFFA_API.Logic.Helpers.MapperExtensions;

namespace CFFA_API.Logic.Implementations
{
    public class PostBehaviour : IPostBehaviour
    {
        public static int pageSize = 15;
        public static int miniThreadSize = 5;
        private static int commentPageSize = 10;

        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUsage usage;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ILogger<PostBehaviour> logger;

        public PostBehaviour(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository, IUsage usage, IWebHostEnvironment hostEnvironment, ILogger<PostBehaviour> logger)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            this.usage = usage;
            this.hostEnvironment = hostEnvironment;
            this.logger = logger;
        }

        public HomeThreadsViewModel GetPostsByThreads(string userId)
        {
            var homeThreads = new HomeThreadsViewModel()
            {
                Popular = new List<PostOnlyViewModel>(),
                Fresh = new List<PostOnlyViewModel>(),
                Feed = new List<PostOnlyViewModel>()
            };
            var topPosts = _postRepository.GetTopVoted(0, miniThreadSize).ToList();
            foreach (Post post in topPosts)
            {
                homeThreads.Popular.Add(post.AsPostOnlyViewModel());
            }

            var freshPosts = _postRepository.GetNewestPosts(0, miniThreadSize).ToList();
            foreach (Post post in freshPosts)
            {
                homeThreads.Fresh.Add(post.AsPostOnlyViewModel());
            }

            if (userId != null) { 
                var feedPosts = _postRepository.GetFeed(userId, 0, miniThreadSize);
                foreach (Post post in feedPosts)
                {
                    homeThreads.Feed.Add(post.AsPostOnlyViewModel());
                }
            }
            return homeThreads;
        }

        public long Create(CreateUpdatePostViewModel model)
        {
            var post = model.AsPost();
            post.Id = 0;
            post.NotSoftDeleted = true;
                //_mapper.Map<Post>(model);
            post.CreationTime = DateTime.UtcNow;
            _postRepository.Add(post);
            return post.Id;
        }

        public bool Update(CreateUpdatePostViewModel model)
        {
            var post = model.AsPost();
                //_mapper.Map<Post>(model);
            //post.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _postRepository.Update(post);
        }

        public PostFullViewModel GetPostFull(long postId, ApplicationUser viewer)
        {
            var post = _postRepository.GetPost(postId);
            if (post == null)
                return null;

            var viewModel = appendDataTo(post, viewer);
            return viewModel;
        }


        public CreateUpdatePostViewModel GetUpdatePostView(long postId)
        {
            var post = _postRepository.GetPost(postId);
            if (post == null) return null;
            var viewModel = post.AsCreateUpdatePostViewModel();
                //_mapper.Map<CreateUpdatePostViewModel>(post);
            return viewModel;
        }

        public PostFullWithCommentsViewModel GetPostFullWithComments(long postId, int pageIndex, string curentUserId)
        {
            var post = _postRepository.GetPost(postId);
            if (post == null)
                return null;

            var user = _userRepository.GetUser(post.UserId).AsProfileViewModel();
            
            var commentsModels = _commentRepository.GetPost_Comments_Users(post.Id, pageIndex, commentPageSize).ToList();
            var comments = new List<CommentViewModel>();
            foreach(Comment comment in commentsModels)
            {
                var profileViewModel = _userRepository.GetUser(comment.UserId).AsProfileViewModel();
                var commentViewModel = comment.AsCommentViewModel();
                commentViewModel.User = profileViewModel;
                comments.Add(commentViewModel);
            }
            //post.Comments = comments;
            var commentsCount = _postRepository.CountCommentsOf(post.Id);
            var viewModel = post.AsPostFullWithCommentsViewModel();
            viewModel.sketch = _postRepository.GetSketch(post.SketchId).AsSketchViewModel();
            viewModel.User = user;
            viewModel.Comments = comments;
            viewModel.ViewerId = curentUserId;
            var (PostUp, PostDown) = Votes(viewModel.Id);
            viewModel.UnderPost = new UnderPostViewModel
            {
                Favorite = null,
                Id = viewModel.Id,
                UpVotes = PostUp,
                DownVotes = PostDown,
                Comments = commentsCount
            };
            if (viewModel.ViewerId != null)
            {
                viewModel.UnderPost.Favorite = HasFavorite(viewModel.ViewerId, postId);
                viewModel.UnderPost.ViewerVote = _postRepository.UserVoted(curentUserId, post.Id);
            }
            if (viewModel.Comments != null)
                foreach (var comment in viewModel.Comments)
                {
                    var (Up, Down) = _commentRepository.GetVotes(comment.CommentId);
                    comment.UpVotes = Up;
                    comment.DownVotes = Down;
                    comment.ViewerId = viewModel.ViewerId;
                    if (viewModel.ViewerId != null)
                        comment.ViewerVoted = _commentRepository.UserVoted(viewModel.ViewerId, comment.CommentId);
                }
            return viewModel;
        }

        public List<PostAuthorViewModel> GetFavoritePostsByUserId(string userId, int pageIndex, int pageSize)
        {
            var posts = _postRepository.GetFavoritePosts(userId, pageIndex, pageSize).ToList();
            var postsViewModel = new List<PostAuthorViewModel>();
            foreach(Post post in posts)
            {
                postsViewModel.Add(post.AsPostAuthorViewModel());
            }
                //_mapper.Map<List<PostAuthorViewModel>>(posts);
            //_mapper.Map(posts, postsViewModel);
            return postsViewModel;
        }

        public List<PostOnlyViewModel> GetPostsOnlyByUserId(string userId, int pageIndex, int pageSize)
        {
            var posts = _postRepository.GetUserPosts(userId, pageIndex, pageSize).ToList();
            var postsViewModel = new List<PostOnlyViewModel>();
            foreach (Post post in posts)
            {
                postsViewModel.Add(post.AsPostOnlyViewModel());
            }
            return postsViewModel;
        }

        public (int Up, int Down) Votes(long postId)
        {
            return _postRepository.Votes(postId);
        }

        public bool HasFavorite(string userId, long postId)
        {
            return _postRepository.HasFavorite(userId, postId);
        }

        public bool TheOwnerIs(long postId, string userId)
        {
            var post = _postRepository.GetPost(postId);
            if (post!=null)
            {
                return post.UserId == userId;
            }
            return false;
        }

        public bool UpVote(string userId, long postId)
        {
            _postRepository.UpdateVote(userId, postId, true);
            return true;
        }

        public bool DownVote(string userId, long postId)
        {
            _postRepository.UpdateVote(userId, postId, false);
            return true;
        }

        public void FavoritePost(string userId, long postId)
        {
            _postRepository.FavoritePosts(userId, postId, true);
        }

        public void UnFavoritePost(string userId, long postId)
        {
            _postRepository.FavoritePosts(userId, postId, false);
        }

        public void Disable(long postId)
        {
            _postRepository.Disable(postId);
        }

        public List<PostFullViewModel> GetPopularPosts(int pageIndex, ApplicationUser viewer)
        {
            var posts = _postRepository.GetTopVoted(pageIndex, pageSize).ToList();
            var viewModel = new List<PostFullViewModel>();
            foreach (Post post in posts)
            {
                var vModel = appendDataTo(post, viewer);
                viewModel.Add(vModel);
            }
            return viewModel;
        }

        public List<PostFullViewModel> GetFreshPosts(int pageIndex, ApplicationUser viewer)
        {
            var posts = _postRepository.GetNewestPosts(pageIndex, pageSize).ToList();
            var viewModel = new List<PostFullViewModel>();
            foreach (Post post in posts)
            {
                var vModel = appendDataTo(post, viewer);
                viewModel.Add(vModel);
            }
            return viewModel;
        }

        public List<PostFullViewModel> GetFeedPosts(ApplicationUser user, int pageIndex)
        {
            var posts = _postRepository.GetFeed(user.Id, pageIndex, pageSize).ToList();
            var viewModel = new List<PostFullViewModel>();
            foreach (Post post in posts)
            {
                var vModel = appendDataTo(post, user);
                viewModel.Add(vModel);
            }
            return viewModel;
        }

        public List<PostOnlyViewModel> Search(string like, int page)
        {
            var posts = _postRepository.Search(like, page, pageSize).ToList();
            var result = new List<PostOnlyViewModel>();
            foreach (Post post in posts)
            {
                result.Add(post.AsPostOnlyViewModel());
            }
            return result;
        }

        public bool PostExists(long postId)
        {
            return _postRepository.GetPost(postId) != null;
        }

        public SketchViewModel CreateSketch(Sketch sketch)
        {
            return _postRepository.CreateSketch(sketch).AsSketchViewModel();
        }

        public string TryCopyOfSketch(long sketchId, int leafs, float height)
        {
            var sketch = _postRepository.GetSketch(sketchId);
            logger.LogDebug($"Sketch exists: {sketch!=null}");
            if (sketch == null)
                return null;
            return usage.Generate(hostEnvironment.WebRootPath, sketch.Id, sketch.Extension, leafs, height);
        }

        private PostFullViewModel appendDataTo(Post post, ApplicationUser viewer)
        {
            ApplicationUser user;
            if (post.UserId == viewer?.Id)
                user = viewer;
            else
                user = _userRepository.GetUser(post.UserId);

            var viewerId = viewer?.Id;
            var commentsCount = _postRepository.CountCommentsOf(post.Id);
            var viewModel = post.AsPostFullViewModel();
            viewModel.sketch = _postRepository.GetSketch(post.SketchId).AsSketchViewModel();
            viewModel.User = user.AsIndexProfileViewModel();
            viewModel.ViewerId = viewerId;
            var (PostUp, PostDown) = Votes(viewModel.Id);
            viewModel.UnderPost = new UnderPostViewModel
            {
                Favorite = null,
                Id = viewModel.Id,
                UpVotes = PostUp,
                DownVotes = PostDown,
                Comments = commentsCount
            };
            if (viewerId != null)
            {
                viewModel.UnderPost.Favorite = HasFavorite(viewerId, post.Id);
                viewModel.UnderPost.ViewerVote = _postRepository.UserVoted(viewerId, post.Id);
            }
            return viewModel;
        }
    }
}
