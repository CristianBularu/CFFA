using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using CFFA_API.Models.ViewModels.Creational;
using System.IO;
using System;
using CFFA_API.Controllers.Helpers;

namespace CFFA_API.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUserBehaviour userBehaviour;
        private readonly IPhotoManager photoManager;

        public UserController(UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IUserBehaviour userBehaviour, IPhotoManager photoManager)
        {
            this.userManager = userManager;
            this.hostEnvironment = hostEnvironment;
            this.userBehaviour = userBehaviour;
            this.photoManager = photoManager;
        }

        [HttpGet]
        public async Task<IActionResult> Guest(string userId, int postPage = 1)
        {
            if (postPage < 1)
                postPage = 1;
            ApplicationUser applicationUser = null;
            if (User.Identity.IsAuthenticated)
                applicationUser = await userManager.GetUserAsync(User);
            if(applicationUser?.Id == userId)
                return RedirectToAction("Profile");
            return await Validation_ModelState_UserExistance(userId, ModelState, () =>
            {
                var user = userBehaviour.GetVisitProfile(userId, applicationUser?.Id??null, postPage - 1);
                if (user != null)
                    return Ok(user);
                return Conflict("User not found");
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(int pageOwnPosts = 1, int pageFavoritePosts = 1, int pageSubscriptions = 1, int pageSketches = 1)
        {
            if (pageOwnPosts < 1)
                pageOwnPosts = 1;
            if (pageFavoritePosts < 1)
                pageFavoritePosts = 1;
            if (pageSubscriptions < 1)
                pageSubscriptions = 1;
            if (pageSketches < 1)
                pageSketches = 1;
            return await Validation_EmailConfirmation_ModelState(User, ModelState, async (user) =>
            {
                var userModel = userBehaviour.GetOwnProfile(user.Id, pageOwnPosts - 1, pageFavoritePosts - 1, pageSubscriptions - 1, pageSketches - 1);
                return Ok(userModel);
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Subscribe(string userId)
        {
            return await Validation_EmailConfirmation_ModelState_UserExistance(User, userId, ModelState, (user) =>
            {
                if (userId == user.Id)
                    return Conflict("Can not subscribe to yourself");

                userBehaviour.SubscribeTo(user.Id, userId);
                return Ok();
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UnSubscribe(string userId)
        {
            return await Validation_EmailConfirmation_ModelState_UserExistance(User, userId, ModelState, (user) =>
            {
                userBehaviour.UnSubscribeTo(user.Id, userId);
                if (userId == user.Id)
                    return Conflict("Can not unsubscribe from yourself");
                return Ok();
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeName([FromBody] ChangeFullNameViewModel viewModel)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, async (user) =>
            {
                userBehaviour.ChangeFullName(user, viewModel.FullName);
                return Ok();
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeIcon([FromForm] ChangeProfilePhoto viewModel)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, async (user) =>
            {
                string extension = await photoManager.SaveProfilePhoto(user.Id, viewModel.File);
                if (extension != null) 
                    if (userBehaviour.ChangeProfilePhotoPath(user, extension)) 
                        return Ok();
                return  Conflict("Could not save file.");
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel viewModel)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, async (user) =>
            {
                var result = await userManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.NewPassword);
                if (!result.Succeeded)
                    return ConflictInternalErrors(result);
                return Ok();
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Disable()
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, async (user) =>
            {
                if (userBehaviour.Disable(user.Id))
                    return Ok();
                return Conflict("Something went wrong.");
            });
        }

        private delegate Task<IActionResult> ValidationDelegateAsyncBasic(ApplicationUser User);
        private delegate IActionResult ValidationDelegateAsync(ApplicationUser User);
        private delegate IActionResult ValidationDelegateAsyncGuest();

        private async Task<IActionResult> Validation_EmailConfirmation_ModelState_UserExistance(ClaimsPrincipal User, string userId, ModelStateDictionary ModelState, ValidationDelegateAsync handler)
        {
            //var user = await userManager.GetUserAsync(User);
            //if (!user.EmailConfirmed)
            //    return Unauthorized("Unconfirmed Email");
            //if (!ModelState.IsValid)
            //    return ConflictFromModelState(ModelState);

            return await Validation_EmailConfirmation_ModelState(User, ModelState, async (user) =>
            {
                if ((await userManager.FindByIdAsync(userId)) == null)
                    return Conflict("User Not found");
                return handler(user);
            });
        }

        private async Task<IActionResult> Validation_ModelState_UserExistance(string userId, ModelStateDictionary ModelState, ValidationDelegateAsyncGuest handler)
        {
            if (!ModelState.IsValid)
                return ConflictFromModelState(ModelState);
            if ((await userManager.FindByIdAsync(userId)) == null)
                return Conflict("User Not found");
            return handler();
        }

        private async Task<IActionResult> Validation_EmailConfirmation_ModelState(ClaimsPrincipal User, ModelStateDictionary ModelState, ValidationDelegateAsyncBasic handler)
        {
            var user = await userManager.GetUserAsync(User);
            if (!user.EmailConfirmed)
                return Conflict("Unconfirmed Email");
            if (!ModelState.IsValid)
                return ConflictFromModelState(ModelState);
            return await handler(user);
        }

        private IActionResult ConflictFromModelState(ModelStateDictionary ModelState)
        {
            string errorMessage = "";
            foreach (ModelStateEntry modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errorMessage += error.ErrorMessage + " ";
                }
            }
            return Conflict(errorMessage);
        }

        private IActionResult ConflictInternalErrors(IdentityResult result)
        {
            string errorMessage = "";

            foreach (IdentityError error in result.Errors)
            {
                errorMessage += error.Description + " ";
            }
            return Conflict(errorMessage);
        }
    }
}
