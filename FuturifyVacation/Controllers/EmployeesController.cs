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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/employees")]
    public class EmployeesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;
        public EmployeesController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context)
        {
            _userManager = userManager;
            db = _context;
        }

        [HttpGet("getall")]
        public List<UserProfile> Get()
        {
            return db.UserProfiles.ToList();
        }

        [HttpGet("{userId}")]
        public UserProfile Get(string userId)
        {
            return db.UserProfiles.Find(userId);
        }

        // POST api/employees/update
        [HttpPost("Update")]
        public IActionResult UpdateProfile([FromBody] ProfileViewModel profile)
        {
            if(ModelState.IsValid)
            {
                var acc = db.Users.FirstOrDefault(x => x.Id == profile.UserId);
                acc.Email = profile.Email;
                acc.PhoneNumber = profile.PhoneNumber;
                db.SaveChanges();

                var userProfile = db.UserProfiles.FirstOrDefault(x => x.UserId == profile.UserId);
                userProfile.FirstName = profile.FirstName;
                userProfile.LastName = profile.LastName;
                userProfile.Gender = profile.Gender;
                userProfile.Position = profile.Position;
                userProfile.DoB = profile.DoB;
                userProfile.Department = profile.Department;
                userProfile.RemainingDayOff = profile.RemainingDayOff;
                db.SaveChanges();
            }
            return Ok();
        }
    }
}
