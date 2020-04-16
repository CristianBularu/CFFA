using CFFA_API.Controllers.Helpers;
using CFFA_API.Logic.Helpers;
using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using CFFA_API.Models.ViewModels.Creational;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CFFA_API.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostBehaviour postBehaviour;
        private readonly IPhotoManager photoManager;

        //private readonly ILogger _logger;

        public PostController(
            IWebHostEnvironment hostEnvironment,
            UserManager<ApplicationUser> userManager,
            IPostBehaviour postBehaviour,
            IPhotoManager photoManager)
        {
            this.hostEnvironment = hostEnvironment;
            this.userManager = userManager;
            this.postBehaviour = postBehaviour;
            this.photoManager = photoManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long postId, int page = 1)
        {
            if (page < 1)
                page = 1;
            string userId = null;
            if (User.Identity.IsAuthenticated)
                userId = (await userManager.GetUserAsync(User)).Id;
            var postViewModel = postBehaviour.GetPostFullWithComments(postId, page - 1, userId);
            if (postViewModel != null)
                return Ok(postViewModel);
            return Conflict("Post not found");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateUpdatePostViewModel viewModel)
        {
            return await Validation_EmailConfirmation_ModelStateAsync(User, ModelState, async (user) => {
                viewModel.UserId = user.Id;
                viewModel.Extension = Path.GetExtension(viewModel.File.FileName);
                var postId = postBehaviour.Create(viewModel);
                string extension = await photoManager.SavePostPhoto($"{postId}", viewModel.File);
                if (extension == null)
                    return Conflict("Could not save the file");
                return StatusCode(201);
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update([FromForm]CreateUpdatePostViewModel viewModel)
        {
            return await Validation_EmailConfirmation_ModelStateAsync(User, ModelState, async (user) => {
                if (postBehaviour.TheOwnerIs(viewModel.Id ??-1, user.Id))
                {
                    string extension = await photoManager.SavePostPhoto($"{viewModel.Id}", viewModel.File);
                    if (extension == null)
                        return Conflict("Could not save the file");
                    viewModel.UserId = user.Id;
                    viewModel.Extension = extension;
                    postBehaviour.Update(viewModel);
                    return StatusCode(201);
                }
                return Unauthorized("Not owning this entity.");
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpVote(long postId)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                if (!postBehaviour.PostExists(postId))
                    return Conflict("Post not found");
                postBehaviour.UpVote(user.Id, postId);
                return Ok();
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DownVote(long postId)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                if (!postBehaviour.PostExists(postId))
                    return Conflict("Post not found");
                postBehaviour.DownVote(user.Id, postId);
                return Ok();
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Favorite(long postId)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                if (!postBehaviour.PostExists(postId))
                    return Conflict("Post not found");
                postBehaviour.FavoritePost(user.Id, postId);
                return Ok();
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UnFavorite(long postId)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                if (!postBehaviour.PostExists(postId))
                    return Conflict("Post not found");
                postBehaviour.UnFavoritePost(user.Id, postId);
                return Ok();
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Disable(long postId)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                if (!postBehaviour.PostExists(postId))
                    return Conflict("Post not found");
                if (postBehaviour.TheOwnerIs(postId, user.Id))
                {
                    postBehaviour.Disable(postId);
                    return Ok();
                }
                return Conflict("Yout are not the owner of this post.");
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSketch([FromForm]CreateUpdateSketchViewModel viewModel)
        {
            return await Validation_EmailConfirmation_ModelStateAsync(User, ModelState, async (user) => {
                viewModel.UserId = user.Id;
                viewModel.Extension = Path.GetExtension(viewModel.File.FileName);
                var sketch = postBehaviour.CreateSketch(viewModel.AsSketch());
                var extension = await photoManager.SaveSketchPhoto($"{sketch.Id}", viewModel.File);
                if (extension == null)
                    return Conflict("Could not save File.");
                return Ok(sketch);
            });
        }

        private delegate IActionResult ValidationDelegate(ApplicationUser User);
        private delegate Task<IActionResult> ValidationDelegateAsync(ApplicationUser User);
        private async Task<IActionResult> Validation_EmailConfirmation_ModelState(ClaimsPrincipal User, ModelStateDictionary ModelState, ValidationDelegate handler)
        {
            var user = await userManager.GetUserAsync(User);
            if(!user.EmailConfirmed)
                return Conflict("Unconfirmed Email");
            if (ModelState.IsValid)
                return handler(user);
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

        private async Task<IActionResult> Validation_EmailConfirmation_ModelStateAsync(ClaimsPrincipal User, ModelStateDictionary ModelState, ValidationDelegateAsync handler)
        {
            var user = await userManager.GetUserAsync(User);
            if (!user.EmailConfirmed)
                return Conflict("Unconfirmed Email");
            if (ModelState.IsValid)
                return await handler(user);
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
    }
}
