using System.Collections.Generic;
using MyTeamWebApi.Model;

namespace MyTeamWebApi.Interfaces
{
    public interface ITeamFactory
    {
        //Gets the list of active teams
        //may filter by tema name and coach name
        List<Team> Get(string teamName = null, string coachName = null);
        //Gets a team data detail, given it's Id
        Team Get(int id);
        //Updates a team's meta data, given its Id and a team instance
        bool Update(int id, Team team);
        //Inserts a new team, in the teams' list
        bool Create(Team team);
        //Adds a new match result type (win, lose, tie) to a team, given it's Id
        bool AddMatch(int teamId, MatchResultType result);
        //Gets the total of matches, by result type (win, lose, tie or all)
        int GetMatchesTotal(int teamId, MatchResultType result);
    }
}