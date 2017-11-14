using FuturifyVacation.Models;
using FuturifyVacation.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.ServicesInterfaces
{
    public interface ISettingService
    {
       
        Task<Setting> GetSetting();
        Task SetDayOff(int numberOfDayOff);
    }
}
