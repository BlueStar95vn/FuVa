using FuturifyVacation.Data;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuturifyVacation.Models;
using FuturifyVacation.Models.ViewModels;
using FuturifyVacation.Models.BindingModels;

namespace FuturifyVacation.Services
{
    public class SettingService : ISettingService
    {
        private ApplicationDbContext _db;

        public SettingService(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<Setting> GetSetting()
        {
            return await _db.Settings.FirstOrDefaultAsync(u => u.Id == 1);
        }

        public async Task SetDayOff(DayOffConfigBindingModel model)
        {
            var getSetting = await _db.Settings.FirstOrDefaultAsync(u => u.Id == 1);
            getSetting.NumberOfDayOff = model.NumberOfDayOff;
            getSetting.HoursADay = model.HoursADay;
            await _db.SaveChangesAsync();
        }

        public async Task SetWorkingTime(SettingViewModel model)
        {
            var hours = 0;
            var getSetting = await _db.Settings.FirstOrDefaultAsync(u => u.Id == 1);
            getSetting.StartAm = model.StartAm;
            getSetting.EndAm = model.EndAm;
            getSetting.StartPm = model.StartPm;
            getSetting.EndPm = model.EndPm;
           
            for (var i = 0; i < 24; i++)
            {
                if (i>= model.StartAm && i < model.EndAm || i >= model.StartPm && i < model.EndPm)
                {
                    hours++;
                }
            }
            getSetting.HoursADay = hours;
            await _db.SaveChangesAsync();
        }
    }
}
