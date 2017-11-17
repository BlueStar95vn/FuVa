using FuturifyVacation.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuturifyVacation.Models;
using FuturifyVacation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FuturifyVacation.Models.ViewModels;

namespace FuturifyVacation.Services
{
    public class ProfileService : IProfileService
    {
        private ApplicationDbContext _db;

        public ProfileService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserProfile> GetByIdAsync(string userId)
        {
            return await _db.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<List<TeamDetail>> GetTeam(string userId)
        {
            return await _db.TeamDetails.Include(u => u.Team).Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<UserProfile> UpdateByIdAsync(ProfileViewModel profile, string userId)
        {
            var userProfile = await _db.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);
            userProfile.FirstName = profile.FirstName;
            userProfile.LastName = profile.LastName;
            userProfile.Gender = profile.Gender;
            userProfile.Position = profile.Position;
            userProfile.DoB = profile.DoB;
            userProfile.Department = profile.Department;
            userProfile.User.PhoneNumber = profile.PhoneNumber;
            userProfile.RemainingDayOff = profile.RemainingDayOff;
            await _db.SaveChangesAsync();
            return userProfile;
        }
    }
}
