using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using CFFA_API.Models.ViewModels;
using CFFA_API.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CFFA_API.Logic.Helpers.MapperExtensions;

namespace CFFA_API.Logic.Implementations
{
    public class UserBehaviour : IUserBehaviour
    {
        private static int MaxConfirmationTokenAtempts = 5;
        private static int MaxResetTokenAtempts = 5;
        private static int MaxRefreshTokenAtempts = 5;
        private static int MaxMinutesAliveConfirmationToken = 5;
        private static int MaxMinutesAliveResetToken = 5;

        private static int UserPostsPageSize = 15;
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepository;

        public delegate void CustomTokensDecorator(CustomTokens customTokens);
        public UserBehaviour(IUserRepository userRepository, IPostRepository postRepository)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;
        }

        public IndexProfileViewModel GetOwnProfile(string userId, int pageOwnPosts, int pageFavoritePosts, int pageSubscriptions, int pageSketches)
        {
            var user = userRepository.GetUser(userId);
            if (user == null)
                return null;
            var userModel = user.AsIndexProfileViewModel();
            var posts = postRepository.GetUserPosts(userId, pageOwnPosts, UserPostsPageSize).ToList();
            var favPosts = postRepository.GetFavoritePosts(userId, pageFavoritePosts, UserPostsPageSize).ToList();
            var subs = userRepository.GetSubscriptions(userId, pageSubscriptions, UserPostsPageSize).ToList();
            var sketches = postRepository.GetUserSketches(userId, pageSketches, UserPostsPageSize).ToList();
            userModel.Posts = new List<PostOnlyViewModel>();
            userModel.FavoritePosts = new List<PostAuthorViewModel>();
            //userModel.Subscriptions = new List<ProfileViewModel>();
            userModel.Sketches = new List<SketchViewModel>();
            foreach (Post post in posts)
            {
                userModel.Posts.Add(post.AsPostOnlyViewModel());
            }
            foreach(Post post in favPosts)
            {
                userModel.FavoritePosts.Add(post.AsPostAuthorViewModel());
            }
            //foreach(ApplicationUser applicationUser in subs)
            //{
            //    userModel.Subscriptions.Add(applicationUser.AsProfileViewModel());
            //}
            foreach (Sketch sketch in sketches)
            {
                userModel.Sketches.Add(sketch.AsSketchViewModel());
            }
            userModel.SubscriptionsCount = userRepository.GetSubscriptionsCount(userId);
            userModel.SubscribersCount = userRepository.GetSubscribersCount(userId);
            userModel.PostsCount = postRepository.GetPostCountOfUser(userId);
            return userModel;
        }

        public ProfileWithPostsViewModel GetVisitProfile(string userId, string currentUserId, int page)
        {
            var user = userRepository.GetUser(userId);
            if (user == null)
                return null;
            var userModel = user.AsProfileWithPostsViewModel();
            if(currentUserId!=null)
                userModel.Subscribed = IsUserSubscribedTo(userId, currentUserId);
            var posts = postRepository.GetUserPosts(userId, page, UserPostsPageSize).ToList();
            foreach(Post post in posts)
            {
                userModel.Posts.Add(post.AsPostOnlyViewModel());
            }
            userModel.SubscriptionsCount = userRepository.GetSubscriptionsCount(userId);
            userModel.PostsCount = postRepository.GetPostCountOfUser(userId);
            userModel.SubscribersCount = userRepository.GetSubscribersCount(userId);
            return userModel;
        }

        public List<ProfileViewModel> GetSubsByUserId(string userId, int pageIndex, int pageSize)
        {
            var users = userRepository.GetSubscriptions(userId, pageIndex, pageSize).ToList();
            var usersViewModel = new List<ProfileViewModel>();
            foreach (ApplicationUser user in users)
            {
                usersViewModel.Add(user.AsProfileViewModel());
            }
            return usersViewModel;
        }

        public void ChangeFullName(ApplicationUser user, string model)
        {
            if (!string.IsNullOrEmpty(model))
                userRepository.ChangeName(user, model);
        }

        public bool ChangeProfilePhotoPath(ApplicationUser user, string ext)
        {
            return userRepository.ChangeProfileIconPath(user, ext);
        }

        public bool IsUserSubscribedTo(string id, string curentUser)
        {
            return userRepository.IsUserSubscribedTo(id, curentUser);
        }

        public void SubscribeTo(string curentUserId, string userId)
        {
            userRepository.AddSubscription(curentUserId, userId);
        }

        public void UnSubscribeTo(string curentUserId, string userId)
        {
            userRepository.RemoveSubscription(curentUserId, userId);
        }

        public bool Disable(string userId)
        {
            return userRepository.Disable(userId);
        }

        public CustomTokenState VerifyCustomToken(string probe, string userId, TokenType type)
        {
            var tokens = userRepository.GetGenerateUserCustomTokens(userId);
            CustomTokenState returnTokenState;
            if (type == TokenType.Confirmation)
            {
                returnTokenState = VerifyConfirmationToken(probe, tokens);
            } else if (type == TokenType.Reset)
            {
                returnTokenState = VerifyResetToken(probe, tokens);
            } else
            {
                returnTokenState = VerifyRefreshToken(probe, userId, tokens);
            }
            return returnTokenState;
        }

        public void ClearTokenNoUpdate(CustomTokens tokens)
        {
            tokens.ConfirmationTokenAttempts = 0;
            tokens.ConfirmationTokenValue = "Empty";
            tokens.ResetPasswordAttempts = 0;
            tokens.ResetPasswordTokenValue = "Empty";
        }

        public void GenerateConfirmationTokenNoUpdate(CustomTokens tokens)
        {
            GenerateCustomConfirmationToken(tokens);
        }

        public List<ProfileViewModel> Search(string like, int page)
        {
            var users = userRepository.Search(like, page, UserPostsPageSize).ToList();
            var result = new List<ProfileViewModel>();
            foreach(ApplicationUser user in users)
            {
                result.Add(user.AsProfileViewModel());
            }
            return result;
        }

        private CustomTokenState VerifyConfirmationToken(string probe, CustomTokens tokens)
        {
            if (tokens.ConfirmationTokenAttempts > MaxConfirmationTokenAtempts)
            {
                return CustomTokenState.SelfDestruct;
            }
            else if (tokens.ConfirmationTokenCreationTime.AddMinutes(MaxMinutesAliveConfirmationToken) < DateTime.UtcNow)
            {
                return CustomTokenState.Expired;
            }
            else if (tokens.ConfirmationTokenValue == "Empty")
            {
                return CustomTokenState.NotCreated;
            }
            else if (probe == tokens.ConfirmationTokenValue)
            {
                //tokens.ConfirmationTokenAttempts = 0;
                //tokens.ConfirmationTokenValue = "Empty";
                //_userRepository.UpdateUserCustomTokens(tokens);
                return CustomTokenState.Valid;
            }
            else
            {
                tokens.ConfirmationTokenAttempts += 1;
                userRepository.UpdateUserCustomTokens(tokens);
                return CustomTokenState.Invalid;
            }
        }

        private CustomTokenState VerifyResetToken(string probe, CustomTokens tokens)
        {
            if (tokens.ResetPasswordTokenValue == "Empty")
            {
                return CustomTokenState.NotCreated;
            }
            else if (tokens.ResetPasswordCreationTime.AddMinutes(MaxMinutesAliveResetToken) < DateTime.UtcNow)
            {
                return CustomTokenState.Expired;
            }
            else if (tokens.ResetPasswordAttempts > MaxResetTokenAtempts)
            {
                return CustomTokenState.SelfDestruct;
            }
            else if (probe == tokens.ResetPasswordTokenValue)
            {
                return CustomTokenState.Valid;
            }
            else
            {
                tokens.ResetPasswordAttempts += 1;
                userRepository.UpdateUserCustomTokens(tokens);
                return CustomTokenState.Invalid;
            }
        }

        private CustomTokenState VerifyRefreshToken(string probe, string userId, CustomTokens tokens)
        {
            if (tokens.RefreshTokenValue == "Empty")
            {
                return CustomTokenState.NotCreated;
            }
            //else if (tokens.ResetPasswordCreationTime.AddMinutes(MaxMinutesAliveResetToken) < DateTime.UtcNow)
            //{
            //    return CustomTokenState.Expired;
            //}
            else if (tokens.RefreshTokenAttempts >= MaxRefreshTokenAtempts)
            {
                return CustomTokenState.SelfDestruct;
            }
            else if (probe == tokens.RefreshTokenValue)
            {
                return CustomTokenState.Valid;
            }
            else
            {
                tokens.RefreshTokenAttempts += 1;
                userRepository.UpdateUserCustomTokens(tokens);
                return CustomTokenState.Invalid;
            }
        }

        public CustomTokens GenerateCustomToken(string userId, TokenType type, CustomTokensDecorator additional = null)
        {
            var tokens = userRepository.GetGenerateUserCustomTokens(userId);
            if (type == TokenType.Confirmation)
            {
                GenerateCustomConfirmationToken(tokens);
            } else if (type == TokenType.Reset)
            {
                GenerateCustomResetToken(tokens);
            } else
            {
                GenerateRefreshToken(tokens);
            }
            additional?.Invoke(tokens);
            userRepository.UpdateUserCustomTokens(tokens);
            return tokens;
        }

        private void GenerateCustomConfirmationToken(CustomTokens tokens)
        {
            tokens.ConfirmationTokenAttempts = 0;
            tokens.ConfirmationTokenCreationTime = DateTime.UtcNow;
            tokens.ConfirmationTokenValue = (new Random()).Next(1000, 9999).ToString();
        }

        private void GenerateCustomResetToken(CustomTokens tokens)
        {
            tokens.ResetPasswordAttempts = 0;
            tokens.ResetPasswordCreationTime = DateTime.UtcNow;
            tokens.ResetPasswordTokenValue = (new Random()).Next(1000, 9999).ToString();
        }

        private void GenerateRefreshToken(CustomTokens tokens)
        {
            tokens.RefreshTokenAttempts = 0;
            tokens.RefreshTokenCreationTime = DateTime.UtcNow;
            tokens.RefreshTokenValue = RandomString(256);
        }


        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "qwertyuiopasdfghjklzxcvbnmABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
