using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FuturifyVacation.Models;
using FuturifyVacation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FuturifyVacation.Models.ViewModels;
using FuturifyVacation.ServicesInterfaces;
using System.Security.Claims;
using FuturifyVacation.Models.BindingModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/profiles")]
    public class ProfilesController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileService _profileService;

        public ProfilesController(UserManager<ApplicationUser> userManager, IProfileService profileService)
        {
            _userManager = userManager;
            _profileService = profileService;
          
        }

        [HttpGet("myId")]
        public async Task<ProfileViewModel> GetAsync(string userId)
        {
            var user = HttpContext.User;
            userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile =  await _profileService.GetByIdAsync(userId);
            return new ProfileViewModel
            {
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Email = profile.User.UserName,
                Gender = profile.Gender,
                Position = profile.Position,
                DoB = profile.DoB,
                Department = profile.Department,
                RemainingDayOff = profile.RemainingDayOff,
                Status = profile.Status,
                PhoneNumber = profile.User.PhoneNumber
            };
        }

        //POST api/profiles/update
       [HttpPost("update")]
        public async Task<ProfileViewModel> UpdateProfile([FromBody] ProfileViewModel profile, string UserId)
        {
            var user = HttpContext.User;
            UserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var update = await _profileService.UpdateByIdAsync(profile, UserId);
           return new ProfileViewModel
           {
               UserId = update.UserId,
               FirstName = update.FirstName,
               LastName = update.LastName,           
               Gender = update.Gender,
               Position = update.Position,
               DoB = update.DoB,
               Department = update.Department,
               RemainingDayOff = update.RemainingDayOff,
               Status = update.Status,
               PhoneNumber = update.User.PhoneNumber
           };
        }
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest();
            }           
            return Ok();
        }
      
        [HttpGet("getmyteam")]
        public async Task<List<MyTeamBindingModel>> GetMyTeam()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var teams = await _profileService.GetTeam(user.Id);
            return teams.Select(p => new MyTeamBindingModel
            {
                TeamId=p.TeamId,
                TeamName=p.Team.TeamName
            }).ToList();
        }
    }
}
