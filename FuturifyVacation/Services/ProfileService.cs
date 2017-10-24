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

        public async Task<UserProfile> UpdateByIdAsync(ProfileViewModel profile, string userId)
        {
            var getProfile = await _db.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);
            getProfile.FirstName = profile.FirstName;
            getProfile.LastName = profile.LastName;
            getProfile.Gender = profile.Gender;
            getProfile.Position = profile.Position;
            getProfile.DoB = profile.DoB;
            getProfile.Department = profile.Department;
            getProfile.User.PhoneNumber = profile.PhoneNumber;
            getProfile.RemainingDayOff = profile.RemainingDayOff;
            await _db.SaveChangesAsync();
            return getProfile;

        }

    }
}
