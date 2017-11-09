using FuturifyVacation.Models;
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
        Task SaveCalendarId(int id, string googleCalendarId);
       
    }
}
