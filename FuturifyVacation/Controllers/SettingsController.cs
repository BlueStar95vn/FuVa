using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FuturifyVacation.Models.ViewModels;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using FuturifyVacation.Models.BindingModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Route("api/settings")]
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet("allsetting")]
        public async Task<SettingViewModel> GetAllSettingAsync(string userId)
        {
            var getSetting = await _settingService.GetSetting();
            return new SettingViewModel
            {
                Id = getSetting.Id,
                NumberOfDayOff = getSetting.NumberOfDayOff,
                DurationInMonth = getSetting.DurationInMonth,
                DurationInWeek = getSetting.DurationInWeek,
                StartAm = getSetting.StartAm,
                EndAm=getSetting.EndAm,
                StartPm=getSetting.StartPm,
                EndPm=getSetting.EndPm,
                HoursADay = getSetting.HoursADay
              
            };
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("setdayoff")]
        public async Task SetDayOff([FromBody]DayOffConfigBindingModel model)
        {
            await _settingService.SetDayOff(model);
        }


        [Authorize(Roles = "ADMIN")]
        [HttpPost("setworkingtime")]
        public async Task SetWoringTime([FromBody]SettingViewModel model)
        {
            await _settingService.SetWorkingTime(model);
        }
    }
}
