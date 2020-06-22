using System;
using System.Threading.Tasks;
using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CFFA_API.Controllers
{
    //[Authorize(JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostBehaviour postBehaviour;
        private readonly IUserBehaviour userBehavior;
        private readonly ILogger<HomeController> logger;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IPostBehaviour postBehaviour,
            IUserBehaviour userBehavior,
            ILogger<HomeController> logger)
        {
            this.userManager = userManager;
            this.postBehaviour = postBehaviour;
            this.userBehavior = userBehavior;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Threads()
        {
            return await TryCatchLog(async () => { 
                string userId = null;
                if (User.Identity.IsAuthenticated)
                {
                    userId = (await userManager.GetUserAsync(User)).Id;
                }
                var viewModel= postBehaviour.GetPostsByThreads(userId);
                return Ok(viewModel);
            });
        }

        [HttpGet]
        public async Task<IActionResult> Popular(int page = 1)
        {
            return await TryCatchLog(async () => { 
                if(page <1)
                    page = 1;

                ApplicationUser user = null;
                if (User.Identity.IsAuthenticated)
                    user = await userManager.GetUserAsync(User);
                var viewModel = postBehaviour.GetPopularPosts(page - 1, user);
                return Ok(viewModel);
            });
        }

        [HttpGet]
        public async Task<IActionResult> Fresh(int page = 1)
        {
            return await TryCatchLog(async () => {
                if (page < 1)
                    page = 1;
                ApplicationUser user = null;
                if (User.Identity.IsAuthenticated)
                    user = await userManager.GetUserAsync(User);
                var viewModel = postBehaviour.GetFreshPosts(page - 1, user);
                return Ok(viewModel);
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Feed(int page = 1)
        {
            return await TryCatchLog(async () => {
                if (page < 1)
                    page = 1;
                var user = await userManager.GetUserAsync(User);
                var viewModel = postBehaviour.GetFeedPosts(user, page - 1);
                return Ok(viewModel);
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchPosts(string like, int page = 1)
        {
            return await TryCatchLog(async () => {
                if (page < 1)
                    page = 1;
                var posts = await postBehaviour.Search(like, page - 1);
                return Ok(posts);
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string like, int page = 1)
        {
            return await TryCatchLog(async () => {
                if (page < 1)
                    page = 1;
                var posts = await userBehavior.Search(like, page - 1);
                return Ok(posts);
            });
        }

        //TODO: Search by Tags

        private delegate Task<IActionResult> CodeBlockDelegate();

        private async Task<IActionResult> TryCatchLog(CodeBlockDelegate handler)
        {
            try
            {
                return await handler();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
