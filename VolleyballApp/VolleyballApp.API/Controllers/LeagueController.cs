using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballApp.API.Data;
using VolleyballApp.API.Dtos;
using VolleyballApp.API.Helpers;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly IVolleyballRepository _repository;
        private readonly IMapper _mapper;
         public LeagueController(IVolleyballRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name= "GetLeague")]
        public async Task<IActionResult> GetLeague(int id)
        {
            var league = await _repository.GetLeague(id);
            var leagueToReturn = _mapper.Map<LeagueForDetailedDto>(league);
            return Ok(leagueToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeagues([FromQuery]UserParams userParams){
            var leagues = await _repository.GetLeagues(userParams);
            var leaguesToReturn = _mapper.Map<List<LeagueForListDto>>(leagues);
            Response.AddPagination(leagues.CurrentPage, leagues.PageSize, leagues.TotalCount, leagues.TotalPages);
            return Ok(leaguesToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeague(LeagueForCreationDto leagueForCreationDto) {
            if (leagueForCreationDto.CreatorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var userCreating = await _repository.GetUser(leagueForCreationDto.CreatorId);
            if (userCreating.OwnedTeam == false && userCreating.UserType == "player") return BadRequest("You need to be owner of the team");
            if (userCreating.IsMailActivated == false) return BadRequest("User account is not activated");
            var createdLeague = await _repository.CreateLeague(leagueForCreationDto);
            var leagueToReturn = _mapper.Map<LeagueForDetailedDto>(createdLeague);
            return CreatedAtRoute("GetLeague", new { controller = "League", id = createdLeague.Id }, leagueToReturn);
        }

        [HttpPut("{userId}/{leagueId}")]
        public async Task<IActionResult> JoinLeague(int userId, int leagueId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var userJoining = await _repository.GetUser(userId);
            var leagueToJoin = await _repository.GetLeague(leagueId);
            if (userJoining.OwnedTeam == false) return BadRequest ("You are not an owner of a team");
            if (leagueToJoin.TeamLeague.Count() >= leagueToJoin.TeamLimit) return BadRequest ("This league is alredy full");
            if (DateTime.Compare(DateTime.Now, leagueToJoin.ClosedSignUp) > 0) return BadRequest("League has already closed its sign up");
            var teams = new List<TeamLeague>(leagueToJoin.TeamLeague);
            if (teams.Select(x => x.TeamId).Contains(userJoining.Team.Id)) return BadRequest("Your team is already in this league");
            await _repository.AddTeamToLeague(userJoining, leagueToJoin);
            Helpers.MailSender.sendLeagueJoinInfo(userJoining.Team.Id, leagueId, leagueToJoin.Creator.Mail);
            return Ok();
        }

        [HttpPut("{leagueId}")]
        public async Task<IActionResult> StartLeague(int leagueId)
        {
            var leagueFromRepo = await _repository.GetLeague(leagueId);
            if (leagueFromRepo.Creator.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            if (leagueFromRepo.TeamLeague.Count() == 1) return BadRequest("League needs at least 2 teams");
            if (leagueFromRepo.Matches.Count()> 0) return BadRequest("League has already started");
            
            await _repository.CreateAndAddMatches(leagueId);
            return Ok();
        }

        [HttpGet("{id}/matches")]
        public async Task<IActionResult> GetLeagueMatches([FromQuery]UserParams userParams, int id)
        {
            var matches = await _repository.GetLeagueMatches(id, userParams);
            var matchesToReturn = _mapper.Map<List<MatchForListDto>>(matches);
            Response.AddPagination(matches.CurrentPage, matches.PageSize, matches.TotalCount, matches.TotalPages);
            return Ok(matchesToReturn);
        }
        
    }
}