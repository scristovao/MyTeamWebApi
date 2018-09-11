using System;
using System.Collections.Generic;
using System.Linq;
using MyTeamWebApi.Interfaces;

namespace MyTeamWebApi.Model
{
    //Class that hold the responsability to persist the application' teams list
    //Provides teams' data operation assuring low level data consistency 
    public class TeamFactory : ITeamFactory
    {
        List<Team> _inMemoryTeams = new List<Team>();

        public TeamFactory()
        {
            _inMemoryTeams = new List<Team> {
                new Team { Id = 1, Name = "Sporting", CoachName = "Damasio" },
                new Team { Id = 2, Name = "Benfica", CoachName = "Andre" },
                new Team { Id = 3, Name = "Beira Mar", CoachName = "Mourinho" },
            };
        }

        public List<Team> Get(string teamName = null, string coachName = null)
        {
            IEnumerable<Team> teams = null;

            if (!String.IsNullOrWhiteSpace(teamName) && !String.IsNullOrWhiteSpace(coachName))
            {
                teams = _inMemoryTeams
                        .Where(x => x.IsActive &&
                            teamName.ToLower() == x.Name.ToLower() &&
                            coachName.ToLower() == x.CoachName.ToLower()
                        );
            }
            else if (!String.IsNullOrWhiteSpace(teamName))
            {
                teams = _inMemoryTeams
                        .Where(x => x.IsActive &&
                            teamName.ToLower() == x.Name.ToLower()
                        );
            }
            else if (!String.IsNullOrWhiteSpace(coachName))
            {
                teams = _inMemoryTeams
                        .Where(x => x.IsActive &&
                            coachName.ToLower() == x.CoachName.ToLower()
                        );
            }
            else
            {
                teams = _inMemoryTeams.Where(x => x.IsActive);
            }

            return teams.ToList();
        }

        public Team Get(int id)
        {
            return _inMemoryTeams.FirstOrDefault(x => x.Id == id);
        }

        public bool Update(int id, Team team)
        {
            var updated = false;

            foreach (var item in _inMemoryTeams)
            {
                if (item.Id == id)
                {
                    item.Name = team.Name ?? item.Name;
                    item.CoachName = team.CoachName ?? item.CoachName;
                    item.IsActive = team.IsActive;
                    updated = true;
                }
            }

            return updated;
        }

        public bool Create(Team team)
        {
            //assuring id and name are unique
            var teamToUpdate = _inMemoryTeams.FirstOrDefault(
                x => x.Id == team.Id || x.Name.ToLower() == team.Name.ToLower()
                );

            if (teamToUpdate != null)
            {
                return false;
            }

            _inMemoryTeams.Add(team);

            return true;
        }

        public bool AddMatch(int teamId, MatchResultType result)
        {
            var teamToUpdate = _inMemoryTeams.FirstOrDefault(x => x.Id == teamId);

            if (teamToUpdate == null)
            {
                return false;
            }

            teamToUpdate.AddMatch(result);

            return true;
        }

        public int GetMatchesTotal(int teamId, MatchResultType result)
        {
            var teamToUpdate = _inMemoryTeams.FirstOrDefault(x => x.Id == teamId);

            if (teamToUpdate == null)
            {
                return 0;
            }

            return teamToUpdate.GetTotals(result);
        }
    }
}