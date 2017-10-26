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

        [HttpPost("bookvacation")]
        public async Task<UserVacationViewModel> BookVacation([FromBody]UserVacationViewModel model, string userId)
        {
            var user = HttpContext.User;
            userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            await _vacationService.AddVacationAsync(model, userId);
            return model;
        }
        [HttpGet("getallvacation")]
        public async Task<List<UserVacationViewModel>> GetAllVacation()
        {
            var getAllVacation = await _vacationService.GetAllVacationAsync();
            return getAllVacation.Select(p => new UserVacationViewModel
            {
               Id = p.Id,
               Title=p.Title,
               Start=p.Start,
               End=p.End,
               UserId=p.UserId
            }).ToList();
        }
    }
}
