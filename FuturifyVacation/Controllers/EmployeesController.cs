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
using FuturifyVacation.Models.BindingModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/employees")]
    public class EmployeesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IEmployeeService _employeeService;
        private readonly IEmailSender _emailSender;

        public EmployeesController(UserManager<ApplicationUser> userManager, IEmployeeService employeeService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _employeeService = employeeService;
            _emailSender = emailSender;
        }

        [HttpGet("getall")]
        public async Task<List<EmployeeViewModel>> GetAllAsync()
        {
            var profiles = await _employeeService.GetAllAsync();
            return profiles.Select(p => new EmployeeViewModel
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
        public async Task<EmployeeViewModel> GetAsync(string userId)
        {
            var profile = await _employeeService.GetByIdAsync(userId);
            return new EmployeeViewModel
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
        public async Task<EmployeeViewModel> UpdateProfile([FromBody] EmployeeViewModel employee, string userId)
        {
            //var user = HttpContext.User;
            //UserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var update = await _employeeService.UpdateByIdAsync(employee, userId);
            return new EmployeeViewModel
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

       
        [HttpPost("register")]
        public async Task<EmployeeViewModel> Register([FromBody]EmployeeViewModel employee)
        {
            DateTime now = DateTime.Now;
            var remainingDayOff = ((13 - now.Month)*8).ToString();
            var user = new ApplicationUser { UserName = employee.Email, Email = employee.Email, PhoneNumber = employee.PhoneNumber };
            user.UserProfile = new UserProfile
            {
                //UserId = user.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Department = employee.Department,
                Gender = employee.Gender,
                RemainingDayOff = remainingDayOff,
                Position="USER"
                

            };

            ////Create account without password
            var resultSaveGoogleEmail = await _userManager.CreateAsync(user);
            if (resultSaveGoogleEmail.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "USER");

                var subject = "[Vacation Tracking] Your account is created";
                var message = "Hi " + employee.FirstName + " " + employee.LastName + "," +
                    "\nLogin with your email! http://localhost:63237/#/login";                   
                await _emailSender.SendEmailAsync(employee.Email, subject, message);
            }

            ////Create account with password
            //var password = GenerateRandomPassword();
            //var result = await _userManager.CreateAsync(user, password);           
            //if (result.Succeeded)
            //{
            //    await _userManager.AddToRoleAsync(user, "USER");

            //    var subject = "[Vacation Tracking] Your account information";
            //    var message = "Hi " + employee.FirstName + " " + employee.LastName + "," +
            //        "\n This is your password: " + password;
            //   await _emailSender.SendEmailAsync(employee.Email, subject, message);
            //}
            return employee;
        }

        [HttpGet("getteam/{userId}")]
        public async Task<List<EmployeeTeamBindingModel>> GetEmployeeTeam(string userId)
        {                       
            var teams = await _employeeService.GetTeam(userId);
            return teams.Select(p => new EmployeeTeamBindingModel
            {
                TeamId = p.TeamId,
                TeamName = p.Team.TeamName
            }).ToList();
        }

        [HttpPost("setdayoff")]
        public async Task SetDayOff([FromBody]DayOffConfigBindingModel model)
        {
            int hours = model.RemainingDayOff * 8;
            await _employeeService.SetDayOff(hours.ToString());
        }
        public string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 2,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[]
            {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
            };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}
