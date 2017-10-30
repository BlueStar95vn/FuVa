using FuturifyVacation.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuturifyVacation.Models;
using FuturifyVacation.Data;
using FuturifyVacation.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FuturifyVacation.Services
{
    public class VacationService : IVacationService
    {
        private ApplicationDbContext _db;

        public VacationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddVacationAsync(UserVacationViewModel model, string userId)
        {
           
            var vacation = new UserVacation
            {
                Id = model.Id,
                Title = model.Title,
                UserId = userId,
                Start = model.Start,
                End = model.End,
                Color=model.Color
            };
            await _db.UserVacations.AddAsync(vacation);
            await _db.SaveChangesAsync();
        }

        public Task<UserVacation> DeleteVacationAsync(int vacationId)
        {
            throw new NotImplementedException();
        }
        public async Task ApproveVacation(int vacationId)
        {
            var getVacation = await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == vacationId);
            getVacation.Color = "Green";
            await _db.SaveChangesAsync();
        }
        public Task DisapproveVacation(string vacationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserVacation>> GetAllVacationAsync()
        {
            return await _db.UserVacations.Include(u=>u.User).ToListAsync();
        }


        public async Task<UserVacation> GetVacationByVacationIdAsync(int vacationId)
        {
           var getVacation =  await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == vacationId);
            return getVacation;
        }
        public async Task<List<UserVacation>> GetRequestVacationAsync()
        {
            return await _db.UserVacations.Include(u=>u.User).Where(u=>u.Color=="blue").ToListAsync();
        }
        public Task<UserVacation> UpdateVacationAsync(int vacationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserVacation>> GetVacationByUserIdAsync(string userId)
        {
            return await _db.UserVacations.Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task DisapproveVacation(int vacationId)
        {
            var vacation = await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == vacationId);
            _db.UserVacations.Remove(vacation);
            await _db.SaveChangesAsync();
        }
    }
}
