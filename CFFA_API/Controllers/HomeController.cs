using System.Threading.Tasks;
using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        //private readonly ILogger _logger;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IPostBehaviour postBehaviour,
            IUserBehaviour userBehavior)
        {
            this.userManager = userManager;
            this.postBehaviour = postBehaviour;
            this.userBehavior = userBehavior;
        }

        [HttpGet]
        public async Task<IActionResult> Threads()
        {
            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = (await userManager.GetUserAsync(User)).Id;
            }
            var viewModel= postBehaviour.GetPostsByThreads(userId);
            return Ok(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Popular(int page = 1)
        {
            if(page <1)
                page = 1;

            ApplicationUser user = null;
            if (User.Identity.IsAuthenticated)
                user = await userManager.GetUserAsync(User);
            var viewModel = postBehaviour.GetPopularPosts(page - 1, user);
            return Ok(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Fresh(int page = 1)
        {
            if (page < 1)
                page = 1;
            ApplicationUser user = null;
            if (User.Identity.IsAuthenticated)
                user = await userManager.GetUserAsync(User);
            var viewModel = postBehaviour.GetFreshPosts(page - 1, user);
            return Ok(viewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Feed(int page = 1)
        {
            if (page < 1)
                page = 1;
            var user = await userManager.GetUserAsync(User);
            var viewModel = postBehaviour.GetFeedPosts(user, page - 1);
            return Ok(viewModel);
        }

        [HttpGet]
        public IActionResult SearchPosts(string like, int page = 1)
        {
            if (page < 1)
                page = 1;
            var posts = postBehaviour.Search(like, page);
            return Ok(posts);
        }

        [HttpGet]
        public IActionResult SearchUsers(string like, int page = 1)
        {
            if (page < 1)
                page = 1;
            var posts = userBehavior.Search(like, page);
            return Ok(posts);
        }

        //TODO: Search by Tags
    }
}
