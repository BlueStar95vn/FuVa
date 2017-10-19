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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/employees")]
    public class EmployeesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IProfileService _profileService;

        public EmployeesController(UserManager<ApplicationUser> userManager, IProfileService profileService)
        {
            _userManager = userManager;
            _profileService = profileService;
        }

        [HttpGet("getall")]
        public async Task<List<ProfileViewModel>> GetAsync()
        {
            var profiles = await _profileService.GetAllAsync();
            return profiles.Select(p => new ProfileViewModel
            {
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.User.Email,
                Gender = p.Gender,
                Position = p.Position,
                DoB = p.DoB,
                Department = p.Department,
                RemainingDayOff = p.RemainingDayOff,
                Status = p.Status,
                PhoneNumber = p.User.PhoneNumber
            }).ToList();
        }

        //[HttpGet("{userId}")]
        //public UserProfile Get(string userId)
        //{
        //    return db.UserProfiles.Find(userId);
        //}
        [HttpGet("{userId}")]
        public async Task<ProfileViewModel> GetByIdAsync(string userId)
        {
            var profile =  await _profileService.GetByIdAsync(userId);
            return new ProfileViewModel
            {
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Email = profile.User.Email,
                Gender = profile.Gender,
                Position = profile.Position,
                DoB = profile.DoB,
                Department = profile.Department,
                RemainingDayOff = profile.RemainingDayOff,
                Status = profile.Status,
                PhoneNumber = profile.User.PhoneNumber
            };
        }

        //POST api/employees/update
       [HttpPost("Update")]
        public async Task<ProfileViewModel> UpdateProfile([FromBody] ProfileViewModel profile, string getUserId)
        {
            return await _profileService.UpdateByIdAsync(profile, getUserId);
        }
    }
}
