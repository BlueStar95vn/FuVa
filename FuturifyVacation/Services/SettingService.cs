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
            var setting = await _db.Settings.FirstOrDefaultAsync(u => u.Id == 1);
            setting.NumberOfDayOff = model.NumberOfDayOff;
            setting.HoursADay = model.HoursADay;
            setting.DurationInMonth = model.DurationInMonth;
            await _db.SaveChangesAsync();
        }

        public async Task SetWorkingTime(SettingViewModel model)
        {
            var hours = 0;
            var setting = await _db.Settings.FirstOrDefaultAsync(u => u.Id == 1);
            setting.StartAm = model.StartAm;
            setting.EndAm = model.EndAm;
            setting.StartPm = model.StartPm;
            setting.EndPm = model.EndPm;
           
            for (var i = 0; i < 24; i++)
            {
                if (i>= model.StartAm && i < model.EndAm || i >= model.StartPm && i < model.EndPm)
                {
                    hours++;
                }
            }
            setting.HoursADay = hours;
            await _db.SaveChangesAsync();
        }
    }
}
