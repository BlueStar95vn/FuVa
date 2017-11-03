using FuturifyVacation.Models;
using FuturifyVacation.Models.BindingModels;
using FuturifyVacation.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.ServicesInterfaces
{
    public interface ITeamService
    {
        Task AddTeamAsync(TeamViewModel model);
        Task RemoveTeamAsync(int teamId);
        Task<List<UserTeam>> GetAllTeamAsync();
        Task<List<TeamDetail>> GetTeamMemberByTeamIdAsync(int teamId);
        Task AddMemberAsync(TeamMemberBindingModel model, int teamId);

        Task<UserTeam> GetDetailByTeamIdAsync(int teamId);


        Task EditTeamNameAsync(int teamId, string teamName);
       
        Task<TeamDetail> RemoveMemberAsync();
        Task<TeamDetail> SetTeamLeadAsync();
    }
}
