using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2.Flows;
using System.Threading;
using FuturifyVacation.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using FuturifyVacation.Models;
using Microsoft.AspNetCore.Authorization;
using FuturifyVacation.ServicesInterfaces;
using System.Security.Claims;
using Google.Apis.Auth.OAuth2.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/googlecalendars")]
    public class GoogleCalendarsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IGoogleService _googleService;


        public GoogleCalendarsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IGoogleService googleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _googleService = googleService;
        }

        [HttpPost("addevent")]
        public async Task<UserVacationViewModel> AddEvent([FromBody]UserVacationViewModel model)
        {          
           // var getUser = await _userManager.GetUserAsync(User);
            await _googleService.AddEvent(model, model.UserId);
            return model;
        }

        [HttpPost("saveeventid/{eventGoogleId}/{eventFullId}")]
        public async Task SaveEventId(string eventGoogleId, int eventFullId)
        {
            await _googleService.SaveEventId(eventGoogleId, eventFullId);
        }

        [HttpPost("update/{eventGoogleId}")]
        public async Task UpdateGoogleEvent([FromBody]UserVacationViewModel model,string eventGoogleId)
        {
            var getUser = await _userManager.GetUserAsync(User);          
            await _googleService.UpdateEvent(model,eventGoogleId, getUser.Id);
        }

        [HttpDelete("deletegoogleevent/{googleEventId}")]
        public async Task DeleteGoogleEvent(string googleEventId)
        {
            var getUser = await _userManager.GetUserAsync(User);            
            await _googleService.DeleteEvent(googleEventId, getUser.Id);
        }

        [HttpPost("approve/{fullEventId}")]
        public async Task ApproveGoogleEvent([FromBody]UserVacationViewModel model, int fullEventId)
        {           
            await _googleService.ApproveVacation(model, fullEventId);
        }
    }
}
