﻿using FuturifyVacation.Models;
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
        Task<List<UserVacation>> GetRequestVacationAsync();
       Task<List<UserVacation>> GetAllVacationAsync();
        Task AddVacationAsync(UserVacationViewModel model, string userId);
        Task<UserVacation> UpdateVacationAsync(int vacationId);
        Task<UserVacation> DeleteVacationAsync(int vacationId);
        Task ApproveVacation(int vacationId);
        Task DisapproveVacation(int vacationId);
    }
}
