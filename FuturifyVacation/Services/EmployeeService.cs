using FuturifyVacation.Data;
using FuturifyVacation.Models;
using FuturifyVacation.Models.ViewModels;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Services
{
    public class EmployeeService : IEmployeeService
    {
        private ApplicationDbContext _db;

        public EmployeeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserProfile>> GetAllAsync()
        {
            return await _db.UserProfiles.Include(u => u.User).ToListAsync();
        }

        public async Task<UserProfile> GetByIdAsync(string userId)
        {
            return await _db.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserProfile> UpdateByIdAsync(EmployeeViewModel employee, string userId)
        {
            var getEmployee = await _db.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);
            getEmployee.FirstName = employee.FirstName;
            getEmployee.LastName = employee.LastName;
            getEmployee.Gender = employee.Gender;
            getEmployee.Position = employee.Position;
            getEmployee.DoB = employee.DoB;
            getEmployee.Department = employee.Department;
            getEmployee.User.PhoneNumber = employee.PhoneNumber;
            getEmployee.RemainingDayOff = employee.RemainingDayOff;
            await _db.SaveChangesAsync();
            return getEmployee;
        }
        public async Task DeleteByIdAsync(string userId)
        {
            var profile = await _db.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);
            //_db.UserProfiles.Remove(profile);
            _db.Users.Remove(profile.User);
            await _db.SaveChangesAsync();
        }

     
    }
}
