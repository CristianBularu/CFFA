using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CFFA_API.Controllers.Helpers.EmailSender;
using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using CFFA_API.Models.ViewModels;
using CFFA_API.Models.ViewModels.Creational;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static CFFA_API.Logic.Implementations.UserBehaviour;
using System.Threading;
using Algorithm;

namespace CFFA_API.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUserBehaviour userBehaviour;

        public AccountController(UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IUserBehaviour userBehaviour)
        {
            this.userManager = userManager;
            this.hostEnvironment = hostEnvironment;
            this.userBehaviour = userBehaviour;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    FullName = viewModel.FullName,
                    CreationTime = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    var token = userBehaviour.GenerateCustomToken(user.Id, TokenType.Confirmation);

                    new Thread(() => {
                        var emailSender = new EmailSender();
                        emailSender.SendEmailConfirmationTokenMessage(user.Email, token.ConfirmationTokenValue);
                    }) {
                        Priority = ThreadPriority.BelowNormal,
                        IsBackground = true
                    }.Start();
                    return Ok();
                }
                else
                {
                    return ConflictInternalErrors(result);
                }
            }
            return ConflictFromModelState(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel resetPassordViewModel)
        {
            return await Validation_ModelState_UserExistance(resetPassordViewModel.Email, ModelState, async (user) => 
            {
                var tokenState = userBehaviour.VerifyCustomToken(resetPassordViewModel.Token, user.Id, TokenType.Reset);
                switch (tokenState)
                {
                    case CustomTokenState.Valid:
                        var token = await userManager.GeneratePasswordResetTokenAsync(user);
                        var result = await userManager.ResetPasswordAsync(user, token, resetPassordViewModel.NewPassword);
                        var authTokens = GetAuthTokens(user);
                        if (result.Succeeded)
                        {
                            return Ok(authTokens);
                        }
                        else
                        {
                            return ConflictInternalErrors(result);
                        }
                    case CustomTokenState.Invalid:
                        return Conflict("Incorrect token.");
                    case CustomTokenState.Expired:
                        return Conflict("Expired token.");
                    case CustomTokenState.SelfDestruct:
                        return Conflict("Destroyed token.");
                    case CustomTokenState.NotCreated:
                        return Conflict("No token.");
                    default:
                        return Conflict();
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> SentResetPasswordToken([FromBody]RequestResetPasswordTokenViewModel requestResetPasswordTokenViewModel)
        {
            return await Validation_ModelState_UserExistance(requestResetPasswordTokenViewModel.Email, ModelState, async (user) => 
            {
                if (!user.EmailConfirmed)
                {
                    return Conflict("Email confirmation is required first.");
                }
                var token = userBehaviour.GenerateCustomToken(user.Id, TokenType.Reset);
                new Thread(() =>
                {
                    var emailSender = new EmailSender();
                    emailSender.SendPasswordResetTokenMessage(requestResetPasswordTokenViewModel.Email, token.ResetPasswordTokenValue);
                })
                {
                    Priority = ThreadPriority.BelowNormal,
                    IsBackground = true
                }.Start();
                return Ok();
            });
        } 

        [HttpPost]
        public async Task<IActionResult> SendEmailConfirmationToken([FromBody]RequestResetPasswordTokenViewModel requestEmailConfirmationTokenViewModel)
        {
            var user = await userManager.FindByEmailAsync(requestEmailConfirmationTokenViewModel.Email);
            if(!user.EmailConfirmed)
            {
                var token = userBehaviour.GenerateCustomToken(user.Id,  TokenType.Confirmation);
                new Thread(() =>
                {
                    var emailSender = new EmailSender();
                    emailSender.SendEmailConfirmationTokenMessage(user.Email, token.ConfirmationTokenValue);
                })
                {
                    Priority = ThreadPriority.BelowNormal,
                    IsBackground = true
                }.Start();
                return Ok();
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailViewModel confirmEmailViewModel)
        {
            return await Validation_ModelState_UserExistance(confirmEmailViewModel.Email, ModelState, async (user) =>
            {
                var tokenState = userBehaviour.VerifyCustomToken(confirmEmailViewModel.Token, user.Id, TokenType.Confirmation);
                switch (tokenState)
                {
                    case CustomTokenState.Valid:
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var result = await userManager.ConfirmEmailAsync(user, token);
                        var authTokens = GetAuthTokens(user);
                        if (result.Succeeded)
                        {
                            return Ok(authTokens);
                        }
                        return ConflictInternalErrors(result);
                    case CustomTokenState.Invalid:
                        return Conflict("Incorrect token.");
                    case CustomTokenState.Expired:
                        return Conflict("Expired token.");
                    case CustomTokenState.SelfDestruct:
                        return Conflict("Destroyed token.");
                    case CustomTokenState.NotCreated:
                        return Conflict("No token.");
                    default:
                        return Conflict();
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody]LoginViewModel loginViewModel)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                return await Validation_ModelState_UserExistance(loginViewModel.Email, ModelState, async (user) =>
                {
                    var authenValid = await userManager.CheckPasswordAsync(user, loginViewModel.Password);
                    if (!authenValid)
                    {
                        return Conflict("Wrong email and password combination.");
                    }
                    if (!user.EmailConfirmed) {
                        var tokens = GetAuthTokens(user, true);
                        return StatusCode(403, tokens);
                    } else { 
                        var tokens = GetAuthTokens(user);
                        return Ok(tokens);
                    }

                });
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenViewModel refreshTokenViewModel)
        {
            var user = await userManager.FindByEmailAsync(refreshTokenViewModel.Email);
            var tokenState = userBehaviour.VerifyCustomToken(refreshTokenViewModel.RefreshToken, user.Id, TokenType.Refresh);
            if (tokenState == CustomTokenState.SelfDestruct)
            {
                return Conflict("Destroyed refresh token. Reauthentication is required.");
            }
            else if (tokenState == CustomTokenState.Invalid)
            {
                return Conflict("Invalid refresh token.");
            }
            else if (tokenState == CustomTokenState.Valid)
            {
                var result = GetAuthTokens(user);
                return Ok(result);
            }
            else
            {
                return Conflict("Unauthentificated. You have not been authentificated yet.");
            }
        }


        [Authorize]
        [HttpGet]
        public IActionResult TestAuthentification()
        {
            string realDirectoryPath = $"{hostEnvironment.WebRootPath}\\Sketch\\19\\o.png";
            var usage = new Usage();

            usage.Generate(realDirectoryPath, 25, 100);
            return Ok();
        }

        private object GetAuthTokens(ApplicationUser user, bool generateConfirmationToken = false)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(issuer: AppSettings.Issuer, audience: AppSettings.Audition, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            CustomTokensDecorator additional = (CustomTokens customTokens) => {
                userBehaviour.ClearTokenNoUpdate(customTokens);
                if (generateConfirmationToken)
                {
                    userBehaviour.GenerateConfirmationTokenNoUpdate(customTokens);
                    new Thread(() =>
                    {
                        var emailSender = new EmailSender();
                        emailSender.SendEmailConfirmationTokenMessage(user.Email, customTokens.ConfirmationTokenValue);
                    })
                    {
                        Priority = ThreadPriority.BelowNormal,
                        IsBackground = true
                    }.Start();
                }
            };

            var refreshToken = userBehaviour.GenerateCustomToken(user.Id, TokenType.Refresh, additional).RefreshTokenValue;
            return new { Id = user.Id, name = user.FullName, extension = user.Extension, email = user.Email, accessToken = accessToken, refreshToken = refreshToken };
        }

        private delegate Task<IActionResult> ValidationDelegateAsync(ApplicationUser User);

        private async Task<IActionResult> Validation_ModelState_UserExistance(string Email, ModelStateDictionary ModelState, ValidationDelegateAsync handler)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(Email);
                if (user != null)
                {
                    return await handler(user);
                }
                return Conflict("User Not found");
            }
            return ConflictFromModelState(ModelState);
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
                if (error.Description.StartsWith("User name "))
                    continue;
                errorMessage += error.Description + " ";
            }
            return Conflict(errorMessage);
        }
    }
}