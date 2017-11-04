using FuturifyVacation.Models;
using FuturifyVacation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.ServicesInterfaces
{
    public interface IProfileService
    {
        Task<UserProfile> GetByIdAsync(string userId);
        Task <UserProfile> UpdateByIdAsync(ProfileViewModel profile,string userId);
        Task<List<TeamDetail>> GetTeam(string userId);
        
    }
}
