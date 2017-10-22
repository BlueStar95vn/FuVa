using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.AspNetCore.Identity;
using FuturifyVacation.Models;
using FuturifyVacation.Models.ViewModels;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/employees")]
    public class EmployeesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IEmployeeService _employeeService;

        public EmployeesController(UserManager<ApplicationUser> userManager, IEmployeeService employeeService)
        {
            _userManager = userManager;
            _employeeService = employeeService;
        }

        [HttpGet("getall")]
        public async Task<List<ProfileViewModel>> GetAllAsync()
        {
            var profiles = await _employeeService.GetAllAsync();
            return profiles.Select(p => new ProfileViewModel
            {
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.User.UserName,
                Gender = p.Gender,
                Position = p.Position,
                DoB = p.DoB,
                Department = p.Department,
                RemainingDayOff = p.RemainingDayOff,
                Status = p.Status,
                PhoneNumber = p.User.PhoneNumber
            }).ToList();
        }

        [HttpGet("{userId}")]
        public async Task<ProfileViewModel> GetAsync(string userId)
        {
            var profile = await _employeeService.GetByIdAsync(userId);
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

        //POST api/employees/update
        [HttpPost("update/{userId}")]
        public async Task<ProfileViewModel> UpdateProfile([FromBody] ProfileViewModel profile, string userId)
        {
            //var user = HttpContext.User;
            //UserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var update = await _employeeService.UpdateByIdAsync(profile, userId);
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
        [HttpDelete("delete/{userId}")]
        public async Task DeleteProfile(string userId)
        {                   
            await _employeeService.DeleteByIdAsync(userId);
        }
    }
}
