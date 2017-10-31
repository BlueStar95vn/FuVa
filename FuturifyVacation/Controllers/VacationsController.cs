using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FuturifyVacation.Models.ViewModels;
using FuturifyVacation.Models;
using Microsoft.AspNetCore.Identity;
using FuturifyVacation.ServicesInterfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FuturifyVacation.Models.BindingModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/vacations")]
    public class VacationsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IVacationService _vacationService;

        public VacationsController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IVacationService vacationService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _vacationService = vacationService;
        }
        //User

        [HttpPost("bookvacation")]
        public async Task<UserVacationViewModel> BookVacation([FromBody]UserVacationViewModel model, string userId)
        {
            var user = HttpContext.User;
            userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            model.Color = "blue";
            await _vacationService.AddVacationAsync(model, userId);
            return model;
        }

        [HttpGet("getuservacation")]
        public async Task<List<UserVacationViewModel>> GetUserVacation()
        {
            var user = HttpContext.User;
            string userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var getUserVacation = await _vacationService.GetVacationByUserIdAsync(userId);
            return getUserVacation.Select(p => new UserVacationViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Start = p.Start,
                End = p.End,
                UserId = p.UserId,
                Color = p.Color
            }).ToList();
        }

        [HttpPost("updateuservacation")]
        public async Task UpdateUserVacation([FromBody] UserVacationViewModel model)
        {
            await _vacationService.UpdateVacationAsync(model);
        }

        //Admin
        [Authorize(Roles = "ADMIN")]
        [HttpGet("getallvacation")]
        public async Task<List<UserVacationViewModel>> GetAllVacation()
        {
            var getAllVacation = await _vacationService.GetAllVacationAsync();
            return getAllVacation.Select(p => new UserVacationViewModel
            {
                Id = p.Id,
                Title = p.User.FirstName + " " + p.User.LastName + ": " + p.Title,
                Start = p.Start,
                End = p.End,
                UserId = p.UserId,
                Color = p.Color

            }).ToList();
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("getrequestvacation")]
        public async Task<List<UserVacationViewModel>> GetRequestVacation()
        {
            var getRequestVacation = await _vacationService.GetRequestVacationAsync();
            return getRequestVacation.Select(p => new UserVacationViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Start = p.Start,
                End = p.End,
                UserId = p.UserId,
                Color = p.Color,
                FirstName = p.User.FirstName,
                LastName = p.User.LastName
            }).ToList();
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost("approve/{Id}")]
        public async Task AprroveVacation(int Id)
        {
            await _vacationService.ApproveVacation(Id);
            
          
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost("disapprove/{Id}")]
        public async Task DisaprroveVacation(int Id, [FromBody]DisapproveBindingModel model)
        {           
            await _vacationService.DisapproveVacation(Id, model.Reason);
        }
    }
}
