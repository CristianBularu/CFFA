﻿using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using CFFA_API.Models.ViewModels;
using CFFA_API.Models.ViewModels.Creational;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CFFA_API.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICommentBehaviour commentBehaviour;
        private readonly ILogger<CommentController> logger;

        public CommentController(
            UserManager<ApplicationUser> userManager,
            ICommentBehaviour commentBehaviour,
            ILogger<CommentController> logger)
        {
            this.userManager = userManager;
            this.commentBehaviour = commentBehaviour;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long postId, int page = 1)
        {
            if(page < 1)
                page = 1;
            return await TryCatchLog(async () => { 
                var result = commentBehaviour.GetComments(postId, page - 1);
                return Ok(result);
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateUpdateCommentViewModel viewModel)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                viewModel.UserId = user.Id;
                if (commentBehaviour.Create(viewModel))
                {
                    return StatusCode(201);
                }
                return Conflict("Could not create comment.");
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody]CreateUpdateCommentViewModel viewModel)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                if(commentBehaviour.TheOwnerIs(viewModel.CommentId ?? -1, user.Id))
                {
                    commentBehaviour.Update(viewModel);
                    return StatusCode(201);
                }
                return Unauthorized("Not owning this entity.");
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpVote(long commentId)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                commentBehaviour.UpVote(user.Id, commentId);
                return Ok();
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DownVote(long commentId)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                commentBehaviour.DownVote(user.Id, commentId);
                return Ok();
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Disable(long commentId)
        {
            return await Validation_EmailConfirmation_ModelState(User, ModelState, (user) => {
                if(commentBehaviour.TheOwnerIs(commentId, user.Id))
                {
                    commentBehaviour.Disable(commentId);
                    return Ok();
                }
                return Unauthorized("Not owning this entity.");
            });
        }

        private delegate IActionResult ValidationDelegate(ApplicationUser User);
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
                //throw;
                return StatusCode(500, "Some internal error wich will not be shown here but will be logged");
            }
        }

        private async Task<IActionResult> Validation_EmailConfirmation_ModelState(ClaimsPrincipal User, ModelStateDictionary ModelState, ValidationDelegate handler)
        {
            return await TryCatchLog(async () => {
                var user = await userManager.GetUserAsync(User);
                if (user.EmailConfirmed)
                {
                    if (ModelState.IsValid)
                    {
                        return handler(user);
                    }
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
                return Conflict("Unconfirmed Email");
            });
        }
    }
}
