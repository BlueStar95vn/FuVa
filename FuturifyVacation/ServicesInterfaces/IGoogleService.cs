using FuturifyVacation.Models;
using FuturifyVacation.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.ServicesInterfaces
{
    public interface IGoogleService
    {
        Task SaveToken(string userId, string token, DateTime issuedAt);
        Task<UserGoogleToken> GetTokenAsync(string userId);
        Task SaveEventId(string googleCalendarId, int id);
        Task AddEvent(UserVacationViewModel model, string userId);
        Task UpdateEvent(UserVacationViewModel model,string googleCalendarId, string userId);
        Task DeleteEvent(string GoogleEventId, string userId);
        Task ApproveVacation(UserVacationViewModel model,int vacationId);
      
    }
}
