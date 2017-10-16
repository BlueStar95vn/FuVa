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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("check-auth")]
        public async Task IsAuthorized()
        {
            var user = HttpContext.User;

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
                    return Ok(new { Success = true });
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
