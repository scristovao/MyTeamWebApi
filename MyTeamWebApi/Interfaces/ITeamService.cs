using System.Collections.Generic;
using MyTeamWebApi.Model;

namespace MyTeamWebApi.Interfaces
{
    public interface ITeamService
    {
        //Gets the list of active teams
        //may filter by team name and/or coach name
        List<Team> GetTeams(string teamName = null, string coachName = null);
        //Gets a team data detail, given it's Id
        Team GetTeam(int id);
        //Updates a team's meta data, given its Id and a team instance
        bool UpdateTeam(int id, Team team);
        //Inserts a new team, in the teams' list
        bool CreateTeam(Team team);
        //Deactivates a team, given it's Id
        bool DeleteTeam(int id);
        //Adds a new match result type (win, lose, tie) to a team, given it's Id
        bool AddMatch(int teamId, MatchResultType result);
        //Gets the total of matches, by result type (win, lose, tie or all)
        int GetMatchesTotal(int teamId, MatchResultType result);
    }
}