using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FuturifyVacation.Models;
using Microsoft.AspNetCore.Authentication;
using FuturifyVacation.Models.ViewModels;
using System.Security.Claims;
using FuturifyVacation.Models.BindingModels;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    //[Authorize]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProfileService _profileService;
        private readonly IGoogleService _googleService;
        private readonly ILogger _logger;

        public AccountController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IProfileService profileService, ILogger<AccountController> logger, IGoogleService googleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileService = profileService;
            _logger = logger;
            _googleService = googleService;
        }

        [HttpGet("check-auth")]
        public async Task<IActionResult> IsAuthorized()
        {
            var getUser = await _userManager.GetUserAsync(User);
                  
            var role = await _userManager.GetRolesAsync(getUser);
           
            var getName = await _profileService.GetByIdAsync(getUser.Id);
            return Ok(new
            {
                Role = role,
                Name = getName.FirstName
                //var user = HttpContext.User;            
                //return Ok(new { userId = user.FindFirstValue(ClaimTypes.NameIdentifier) });        
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            var acc = new ApplicationUser();
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true     

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok(new { Success = true, email = model.Email });
                }
                else
                {
                    return BadRequest(new { Success = false, Error = "Invalid login attempt." });
                }
            }
            return BadRequest(new { Success = false, Error = "asdjaks" });
        }


        [HttpPost("externallogin")]
        [AllowAnonymous]      
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }



        [HttpGet("externallogincallback")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var redirectUrl = "/#/login/";
            if (remoteError != null)
            {
                return BadRequest(new { Success = false, Error = "Error from external provider" });

            }
            var info = await _signInManager.GetExternalLoginInfoAsync();            
            var gettoken = info.AuthenticationTokens.FirstOrDefault(u => u.Name == "access_token");
            var accessToken = gettoken.Value;
            var getExpireTime = info.AuthenticationTokens.FirstOrDefault(u => u.Name == "expires_at");
            var expireTime = getExpireTime.Value;

            
            if (info == null)
            {               
                return Redirect(redirectUrl);
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(email);
                DateTime issuedAt = DateTime.Now;
                await _googleService.SaveToken(user.Id, accessToken, issuedAt);
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                
                redirectUrl = "/#/status";
                return Redirect(redirectUrl);
            }
            if (result.IsLockedOut)
            {               
                return Redirect(redirectUrl);
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(email);
                if(user==null)
                {                   
                    return Redirect(redirectUrl);
                }

                var addProvider = await _userManager.AddLoginAsync(user, info);               
               
                if (addProvider.Succeeded)
                {
                    DateTime issuedAt = DateTime.Now;
                    await _googleService.SaveToken(user.Id, accessToken, issuedAt);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                    redirectUrl = "/#/status";
                    return Redirect(redirectUrl);
                }               
                return Redirect(redirectUrl); //redirect ve trang index
            }
        
        }
   

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Ok();
        }
      
    }
}
