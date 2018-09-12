using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MyTeamWebApi.Globals;
using MyTeamWebApi.Interfaces;

namespace MyTeamWebApi.Model
{
    //Class responsible for providing application's actions over the team's list
    //Handles business logic like second line validations
    public class TeamService : ITeamService
    {
        private readonly ITeamFactory _teamFactory;
        private readonly ILogger<TeamService> _logger;

        public TeamService(ITeamFactory teamFactory, ILogger<TeamService> logger)
        {
            _teamFactory = teamFactory;
            _logger = logger;
        }

        public List<Team> GetTeams(string teamName = null, string coachName = null)
        {
            return _teamFactory.Get(teamName, coachName);
        }

        public Team GetTeam(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("TeamService.GetTeam: " + TextResources.InvalidTeamId);
                return null;
            }

            return _teamFactory.Get(id);
        }

        public bool CreateTeam(Team team)
        {
            if (team == null || !team.IsValid)
            {
                _logger.LogWarning("TeamService.CreateTeam: " + TextResources.InvalidTeamInstance);
                return false;
            }

            return _teamFactory.Create(team);
        }

        public bool UpdateTeam(int id, Team team)
        {
            if (id <= 0)
            {
                _logger.LogWarning("TeamService.UpdateTeam: " + TextResources.InvalidTeamId);
                return false;
            }

            if (team == null)
            {
                _logger.LogWarning("TeamService.UpdateTeam: " + TextResources.NullTeamInstance);
                return false;
            }

            if (String.IsNullOrWhiteSpace(team.Name))
            {
                _logger.LogWarning("TeamService.UpdateTeam: " + TextResources.EmptyTeamName);
                return false;
            }

            var existingTeam = GetTeam(id);

            if (existingTeam == null)
            {
                _logger.LogWarning("TeamService.UpdateTeam: " + TextResources.NullTeamInstance);
                return false;
            }

            return _teamFactory.Update(id, team);
        }

        public bool DeleteTeam(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("TeamService.DeleteTeam: " + TextResources.InvalidTeamId);
                return false;
            }

            var team = GetTeam(id);

            if (team == null)
            {
                _logger.LogWarning("TeamService.DeleteTeam: " + TextResources.NullTeamInstance);
                return false;
            }

            team.IsActive = false;
            return _teamFactory.Update(id, team);
        }

        public bool AddMatch(int teamId, MatchResultType result)
        {
            if (teamId <= 0 || result.Equals(MatchResultType.All))
            {
                _logger.LogWarning("TeamService.AddMatch: " + TextResources.InvalidTeamInstance);
                return false;
            }

            return _teamFactory.AddMatch(teamId, result);
        }

        public int GetMatchesTotal(int teamId, MatchResultType result)
        {
            if (teamId <= 0)
            {
                _logger.LogWarning("TeamService.GetMatchesTotal: " + TextResources.InvalidTeamId);
                return 0;
            }

            return _teamFactory.GetMatchesTotal(teamId, result);
        }
    }
}