using FuturifyVacation.Data;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuturifyVacation.Models;
using FuturifyVacation.Models.ViewModels;

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

        public async Task SetDayOff(int numberOfDayOff)
        {
           var getSetting =  await _db.Settings.FirstOrDefaultAsync(u => u.Id == 1);
            getSetting.NumberOfDayOff = numberOfDayOff;
            await _db.SaveChangesAsync();
        }

    
    }
}
