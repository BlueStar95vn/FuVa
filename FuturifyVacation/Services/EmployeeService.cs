using FuturifyVacation.Data;
using FuturifyVacation.Models;
using FuturifyVacation.Models.BindingModels;
using FuturifyVacation.Models.ViewModels;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeeService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
            var employeeInfo = await _db.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);

            if (employeeInfo.Position != employee.Position)
            {
                await _userManager.RemoveFromRoleAsync(employeeInfo.User, employeeInfo.Position);
            }
            employeeInfo.FirstName = employee.FirstName;
            employeeInfo.LastName = employee.LastName;
            employeeInfo.Gender = employee.Gender;
            employeeInfo.Position = employee.Position;
            if (employee.Position == "ADMIN")
            {
                await _userManager.AddToRoleAsync(employeeInfo.User, "ADMIN");
            }
            else if (employee.Position == "USER")
            {
                await _userManager.AddToRoleAsync(employeeInfo.User, "USER");
            }
            employeeInfo.DoB = employee.DoB;
            employeeInfo.Department = employee.Department;
            employeeInfo.User.PhoneNumber = employee.PhoneNumber;
            employeeInfo.RemainingDayOff = employee.RemainingDayOff;
            await _db.SaveChangesAsync();
            return employeeInfo;
        }
        public async Task DeleteByIdAsync(string userId)
        {
          

            var teams = await _db.TeamDetails.Where(u => u.UserId == userId).ToListAsync();
            if (teams != null)
            {
                _db.TeamDetails.RemoveRange(teams);
            }

            var getGoogle = await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.UserId==userId);
            if(getGoogle!=null)
            {
                _db.UserGoogleToken.Remove(getGoogle);
            }
            var vacations = await _db.UserVacations.Where(u => u.User.UserId == userId).ToListAsync();
            if (vacations != null)
            {
                _db.UserVacations.RemoveRange(vacations);
            }

            var profile = await _db.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);
            //_db.UserProfiles.Remove(profile);
            _db.Users.Remove(profile.User);

            var externalAccount = await _db.UserLogins.FirstOrDefaultAsync(u => u.UserId == userId);
            if (externalAccount != null)
            {
                _db.UserLogins.Remove(externalAccount);
            }

           

            await _db.SaveChangesAsync();
        }

        public async Task<List<TeamDetail>> GetTeam(string userId)
        {
            return await _db.TeamDetails.Include(u => u.Team).Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task SetDayOff(int dayoff)
        {
            var allMember = await _db.UserProfiles.ToListAsync();
            foreach (var mem in allMember)
            {
                //Transfer days off of employees in the previous year to next year
                if (int.Parse(mem.RemainingDayOff) > 0)
                {
                    mem.RemainingDayOff = (int.Parse(mem.RemainingDayOff) + dayoff).ToString();
                }
                else
                {
                    mem.RemainingDayOff = dayoff.ToString();
                }

                ////not transfer
                //mem.RemainingDayOff = dayoff;

            }
            await _db.SaveChangesAsync();
        }
    }
}
