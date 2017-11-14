using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FuturifyVacation.Models.ViewModels;
using FuturifyVacation.ServicesInterfaces;

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
                EndHour = getSetting.EndHour,
                StartHour = getSetting.StartHour
            };
        }


        [HttpPost("setdayoff")]
        public async Task SetDayOff([FromBody]SettingViewModel model)
        {
            await _settingService.SetDayOff(model.NumberOfDayOff);
        }
    }
}
