﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTeamWebApi.Globals;
using MyTeamWebApi.Interfaces;
using MyTeamWebApi.Model;

namespace MyTeamWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(ITeamService teamService, ILogger<TeamsController> logger)
        {
            _teamService = teamService;
            _logger = logger;
        }

        // GET api/team?name=test team&coach=mourinho
        //Returns an id & name list for all active teams
        //may filter by team name and coach name
        [HttpGet]
        public ActionResult Get(string name = null, string coach = null)
        {
            var teamList = _teamService.GetTeams(name, coach);
            return new ObjectResult(
                teamList.Select(
                x => new { id = x.Id, name = x.Name, coach = x.CoachName }
                )
            );
        }

        //GET api/team/1
        //Gets a team's detail data, given it's Id
        [HttpGet("{id}")]
        public ActionResult<Team> Get(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("TeamsController.Get: " + TextResources.InvalidTeamId);
                return BadRequest();
            }

            var team = _teamService.GetTeam(id);

            return team != null? new ObjectResult(team): (ActionResult)NotFound();
        }

        // GET api/team/1/match/resultType
        // Gets a team's total of mathes won, lost or tied, given it's Id and result type 
        [HttpGet("{id}/match/{matchresulttype?}")]
        public ActionResult<int> GetMatchTotals(int id, MatchResultType? matchresulttype)
        {
            if (id <= 0)
            {
                _logger.LogWarning("TeamsController.GetMatchTotals: " + TextResources.InvalidTeamId);
                return BadRequest();
            }

            var team = _teamService.GetTeam(id);

            if (team == null)
            {
                return NotFound();
            }

            var totals = _teamService.GetMatchesTotal(id, matchresulttype ?? MatchResultType.All);
            return totals;
        }

        // POST api/team
        // Inserts a new team into teams' list
        [HttpPost]
        public IActionResult Post([FromBody] Team team)
        {
            if (team == null || team.Id <= 0)
            {
                _logger.LogWarning("TeamsController.Post: " + TextResources.InvalidTeamInstance);
                return BadRequest();
            }

            var result = _teamService.CreateTeam(team);
            return result ? (StatusCodeResult)Ok() : (StatusCodeResult)UnprocessableEntity();
        }

        // PUT api/team/1
        // Updates a team's metadata, given it's Id
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Team team)
        {
            if (id <= 0 || team == null)
            {
                _logger.LogWarning("TeamsController.Put: " + TextResources.InvalidTeamInstance);
                return BadRequest();
            }

            var result = _teamService.UpdateTeam(id, team);
            return result ? (StatusCodeResult)Ok() : (StatusCodeResult)NotFound();
        }

        // PUT api/team/1/match/<all|win|lose|tie>
        // Add a new match result to a team's matches list, given it's Id and result type
        [HttpPut("{id}/match/{matchresulttype}")]
        public IActionResult PutMatch(int id, MatchResultType matchresulttype)
        {
            if (id <= 0)
            {
                _logger.LogWarning("TeamsController.PutMatch: " + TextResources.InvalidTeamId);
                return BadRequest();
            }

            var result = _teamService.AddMatch(id, matchresulttype);
            return result ? (StatusCodeResult)Ok() : (StatusCodeResult)NotFound();
        }

        // DELETE api/team/1
        [HttpDelete("{id}")]
        // Deactivates a team, given it's Id
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("TeamsController.Delete: " + TextResources.InvalidTeamId);
                return BadRequest();
            }

            var result = _teamService.DeleteTeam(id);
            return result ? (StatusCodeResult)Ok() : (StatusCodeResult)NotFound();
        }
    }
}
