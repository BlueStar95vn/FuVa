﻿using FuturifyVacation.Models;
using FuturifyVacation.Models.BindingModels;
using FuturifyVacation.Models.ViewModels;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/teams")]
    public class TeamsController
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("addteam")]
        public async Task AddTeam([FromBody]TeamViewModel model)
        {
            await _teamService.AddTeamAsync(model);

        }
        [HttpGet("getallteam")]
        public async Task<List<TeamViewModel>> GetAllTeam()
        {
            var getTeams = await _teamService.GetAllTeamAsync();
            return getTeams.Select(p => new TeamViewModel
            {
                Id = p.Id,
                TeamName = p.TeamName,
                TeamLeadId = p.TeamLeadId,
                FirstName = p.Profile.FirstName,
                LastName = p.Profile.LastName
            }).ToList();
        }
        [HttpDelete("delete/{teamId}")]
        public async Task GetAllTeam(int teamId)
        {
            await _teamService.RemoveTeamAsync(teamId);
        }

        [HttpGet("getteammember/{teamId}")]
        public async Task<List<TeamMemberBindingModel>> GetAllMember(int teamId)
        {
            var getMems = await _teamService.GetTeamMemberByTeamIdAsync(teamId);
            return getMems.Select(p => new TeamMemberBindingModel
            {
                TeamId = p.TeamId,
                FirstName = p.Profile.FirstName,
                LastName = p.Profile.LastName,
                RoleInTeam = p.RoleInTeam
            }).ToList();
        }
        [HttpPost("addmember/{teamId}")]
        public async Task AddMember([FromBody]TeamMemberBindingModel model, int teamId)
        {
            await _teamService.AddMemberAsync(model, teamId);
        }


        [HttpGet("getteamdetail/{teamId}")]
        public async Task<TeamViewModel> GetTeamDetail(int teamId)
        {
            var getDetail = await _teamService.GetDetailByTeamIdAsync(teamId);
            return new TeamViewModel
            {
                Id = getDetail.Id,
                TeamName = getDetail.TeamName,
                TeamLeadId = getDetail.TeamLeadId,
                FirstName = getDetail.Profile.FirstName,
                LastName = getDetail.Profile.LastName
            };
        }

        [HttpPost("changeteamname/{teamId}")]
        public async Task ChangeTeamName([FromBody]TeamViewModel model, int teamId)
        {
            await _teamService.EditTeamNameAsync(teamId,model.TeamName);
        }

    }
}
