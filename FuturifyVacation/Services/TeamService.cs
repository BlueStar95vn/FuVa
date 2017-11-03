using FuturifyVacation.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuturifyVacation.Models;
using FuturifyVacation.Data;
using FuturifyVacation.Models.BindingModels;
using FuturifyVacation.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FuturifyVacation.Services
{
    public class TeamService : ITeamService
    {
        private ApplicationDbContext _db;

        public TeamService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddMemberAsync(TeamMemberBindingModel model, int teamId)
        {
            var mem = new TeamDetail
            {
                TeamId = teamId,
                UserId = model.UserId,
                RoleInTeam = "Member"
            };
            await _db.TeamDetails.AddAsync(mem);
            await _db.SaveChangesAsync();
        }

        public async Task AddTeamAsync(TeamViewModel model)
        {
            var team = new UserTeam
            {
                TeamName = model.TeamName,
                TeamLeadId = model.UserId
            };

            var addteam = await _db.UserTeams.AddAsync(team);
            if (addteam != null && team.TeamLeadId != null)
            {
                var teamlead = new TeamDetail
                {
                    TeamId = team.Id,
                    RoleInTeam = "Team Lead",
                    UserId = model.UserId,
                };
                await _db.TeamDetails.AddAsync(teamlead);
            }

            await _db.SaveChangesAsync();

        }
        public async Task RemoveTeamAsync(int teamId)
        {
            var getTeamDetail = _db.TeamDetails.Where(u => u.TeamId == teamId);
            _db.TeamDetails.RemoveRange(getTeamDetail);
            var getTeam = await _db.UserTeams.FirstOrDefaultAsync(u => u.Id == teamId);
            _db.UserTeams.Remove(getTeam);
            await _db.SaveChangesAsync();
        }

        public async Task EditTeamNameAsync(int teamId, string teamName)
        {
            var getTeam = await _db.UserTeams.FirstOrDefaultAsync(u => u.Id == teamId);
            getTeam.TeamName = teamName;
            await _db.SaveChangesAsync();
        }

        public async Task<List<UserTeam>> GetAllTeamAsync()
        {
            return await _db.UserTeams.Include(u => u.Profile).ToListAsync();
        }

        public async Task<UserTeam> GetDetailByTeamIdAsync(int teamId)
        {
             return await _db.UserTeams.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == teamId);
        }

        public Task<TeamDetail> RemoveMemberAsync()
        {
            throw new NotImplementedException();
        }



        public Task<TeamDetail> SetTeamLeadAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TeamDetail>> GetTeamMemberByTeamIdAsync(int teamId)
        {
            return await _db.TeamDetails.Include(u=>u.Profile).Where(u => u.TeamId == teamId && u.RoleInTeam == "Member").ToListAsync();

        }
    }
}
