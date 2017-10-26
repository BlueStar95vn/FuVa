using FuturifyVacation.Models;
using FuturifyVacation.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.ServicesInterfaces
{
    public interface IVacationService
    {
        Task<UserVacation> GetVacationByIdAsync(string userId);
        Task<List<UserVacation>> GetAllVacationAsync();
        Task AddVacationAsync(UserVacationViewModel model, string userId);
        Task<UserVacation> UpdateVacationAsync(string vacationId);
        Task<UserVacation> DeleteVacationAsync(string vacationId);
    }
}
