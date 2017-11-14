using FuturifyVacation.Models;
using FuturifyVacation.Models.BindingModels;
using FuturifyVacation.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.ServicesInterfaces
{
    public interface IVacationService
    {
        Task<List<UserVacation>> GetVacationByUserIdAsync(string userId);
        Task<UserVacation> GetVacationByVacationIdAsync(int vacationId); 
        Task<UserVacation> AddVacationAsync(UserVacationViewModel model, string userId);
        Task<UserVacation> UpdateVacationAsync(UserVacationViewModel model);
        Task CancelVacationAsync(int vacationId );

        Task<List<UserVacation>> GetRequestVacationAsync();
        Task<List<UserVacation>> GetAllVacationAsync();
        Task ApproveVacation(int vacationId);
        Task DisapproveVacation(int vacationId, string reason);
    }
}
