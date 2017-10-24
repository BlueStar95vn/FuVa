using FuturifyVacation.Models;
using FuturifyVacation.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.ServicesInterfaces
{
    public interface IEmployeeService
    {
        Task<List<UserProfile>> GetAllAsync();
        Task<UserProfile> GetByIdAsync(string userId);
        Task<UserProfile> UpdateByIdAsync(EmployeeViewModel employee, string userId);
        Task DeleteByIdAsync(string userId);
       
    }
}
