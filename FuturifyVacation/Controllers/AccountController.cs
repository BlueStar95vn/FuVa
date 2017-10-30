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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProfileService _profileService;
        public AccountController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IProfileService profileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileService = profileService;
        }

        [HttpGet("check-auth")]
        public async Task<IActionResult> IsAuthorized()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);           
            var role = await _userManager.GetRolesAsync(user);
            var getUser = await _userManager.GetUserAsync(User);
            var getName = await _profileService.GetByIdAsync(getUser.Id);
            


            return Ok(new
            {
                Role = role.FirstOrDefault(),
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
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Ok();
        }
      
    }
}
