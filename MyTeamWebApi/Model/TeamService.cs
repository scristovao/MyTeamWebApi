using System.Collections.Generic;
using MyTeamWebApi.Interfaces;

namespace MyTeamWebApi.Model
{
    //Class responsible for providing applications' actions over the teams' list
    //Handles business logic like second line validations
    public class TeamService : ITeamService
    {
        TeamFactory _teamFactory = new TeamFactory();

        public List<Team> GetTeams(string teamName = null, string coachName = null)
        {
            return _teamFactory.Get(teamName, coachName);
        }

        public Team GetTeam(int id)
        {
            return _teamFactory.Get(id);
        }

        public bool UpdateTeam(int id, Team team)
        {
            if (team == null)
            {
                return false;
            }

            return _teamFactory.Update(id, team);
        }

        public bool DeleteTeam(int id)
        {
            if (id <= 0)
            {
                return false;
            }

            var team = GetTeam(id);

            if (team == null)
            {
                return false;
            }

            team.IsActive = false;
            return _teamFactory.Update(id, team);
        }

        public bool CreateTeam(Team team)
        {
            if (team == null || team.Id <= 0 || !team.IsValid)
            {
                return false;
            }

            return _teamFactory.Create(team);
        }

        public bool AddMatch(int teamId, MatchResultType result)
        {
            if (teamId <= 0 || result.Equals(MatchResultType.All))
            {
                return false;
            }

            return _teamFactory.AddMatch(teamId, result);
        }

        public int GetMatchesTotal(int teamId, MatchResultType result)
        {
            if (teamId <= 0)
            {
                return 0;
            }

            return _teamFactory.GetMatchesTotal(teamId, result);
        }
    }
}