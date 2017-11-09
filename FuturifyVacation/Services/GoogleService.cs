using FuturifyVacation.Data;
using FuturifyVacation.Models;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Services
{
    public class GoogleService : IGoogleService
    {
        private ApplicationDbContext _db;

        public GoogleService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserGoogleToken> GetTokenAsync(string userId)
        {
            return await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task SaveCalendarId(int id, string googleCalendarId)
        {
            var getVacation =await  _db.UserVacations.FirstOrDefaultAsync(u => u.Id == id);
            getVacation.GoogleCalendarId = googleCalendarId;
            await _db.SaveChangesAsync();
        }

        public async Task SaveToken(string userId, string token, DateTime issuedAt)
        {
            var getToken = await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.UserId == userId);
            if(getToken==null)
            {
                var newToken = new UserGoogleToken
                {
                    UserId = userId,
                    AccessToken = token,
                    IssuedAt = issuedAt.ToUniversalTime()
                };               
                await _db.UserGoogleToken.AddAsync(newToken);              
            }
            else
            {
                getToken.AccessToken = token;
                getToken.IssuedAt = issuedAt.ToUniversalTime();
            }
            await _db.SaveChangesAsync();
        }
    }
}
