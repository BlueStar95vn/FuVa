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
                End = model.End
            };
            await _db.UserVacations.AddAsync(vacation);
            await _db.SaveChangesAsync();
        }

        public Task<UserVacation> DeleteVacationAsync(string vacationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserVacation>> GetAllVacationAsync()
        {
            return await _db.UserVacations.ToListAsync();
        }

        public Task<UserVacation> GetVacationByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserVacation> UpdateVacationAsync(string vacationId)
        {
            throw new NotImplementedException();
        }
    }
}
