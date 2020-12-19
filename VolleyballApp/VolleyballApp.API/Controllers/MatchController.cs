using System;
using System.Collections.Generic;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {

        private readonly IVolleyballRepository _repository;
        private readonly IMapper _mapper;

        public MatchController(IVolleyballRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name= "GetMatch")]
        public async Task<IActionResult> GetMatch(int id)
        {
            var match = await _repository.GetMatch(id);
            var matchToReturn = _mapper.Map<MatchForDetailedDto>(match);
            return Ok(matchToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetMatches([FromQuery]UserParams userParams){
            var matches = await _repository.GetMatches(userParams);
            var matchesToReturn = _mapper.Map<List<MatchForListDto>>(matches);
            Response.AddPagination(matches.CurrentPage, matches.PageSize, matches.TotalCount, matches.TotalPages);
            return Ok(matchesToReturn);
        }

        [HttpPut("{id}/location")]
        public async Task<IActionResult> SetMatchLocationAndTime(LocationForAddDto locationForAddDto, int id)
        {
            var match = await _repository.GetMatch(id);
            var userSendingLocation = await _repository.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if(userSendingLocation.Id != match.FirstTeam.OwnerId) return BadRequest("You are not owner of team that is hosting game");
            if(locationForAddDto.TimeOfMatch < DateTime.Now) return BadRequest("Match date needs to be in future");
            var location = await _repository.AddLocation(locationForAddDto, id);
            return Ok(location);
        }

        [HttpPut("{id}/score")]
        public async Task<IActionResult> SetMatchScore(ScoreForAddDto scoreForAdd, int id)
        {
            var userSendingScore = await _repository.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var matchToAddScore = await _repository.GetMatch(id);
            if (userSendingScore.UserType != "referee") return BadRequest ("You are not a referee");
            if (userSendingScore.Id != matchToAddScore.Referee.Id) return BadRequest("You are not a referee for this match");
            if (matchToAddScore.Score.FirstTeamSets + matchToAddScore.Score.SecondTeamSets != 0) return BadRequest("Score already added");
            if (scoreForAdd.FirstTeamSets != 3 && scoreForAdd.SecondTeamSets !=3) return BadRequest("Wrong score");
            if (scoreForAdd.FirstTeamSets + scoreForAdd.SecondTeamSets == 3 && (scoreForAdd.FourFirstTeam + scoreForAdd.FourSecondTeam + scoreForAdd.FiveFirstTeam + scoreForAdd.FiveSecondTeam != 0)) return BadRequest("Wrong score");
            if (scoreForAdd.FirstTeamSets + scoreForAdd.SecondTeamSets == 4 && (scoreForAdd.FiveFirstTeam + scoreForAdd.FiveSecondTeam != 0)) return BadRequest("Wrong score");
            if (!Helpers.Extension.IsCorrectSet(scoreForAdd.OneFirstTeam, scoreForAdd.OneSecondTeam,1)) return BadRequest("Wrong score");
            if (!Helpers.Extension.IsCorrectSet(scoreForAdd.TwoFirstTeam, scoreForAdd.TwoSecondTeam,2)) return BadRequest("Wrong score");
            if (!Helpers.Extension.IsCorrectSet(scoreForAdd.ThreeFirstTeam, scoreForAdd.ThreeSecondTeam,3)) return BadRequest("Wrong score");
            if (!Helpers.Extension.IsCorrectSet(scoreForAdd.FourFirstTeam, scoreForAdd.FourSecondTeam,4) && scoreForAdd.FirstTeamSets + scoreForAdd.SecondTeamSets >= 4) return BadRequest("Wrong score");
            if (!Helpers.Extension.IsCorrectSet(scoreForAdd.FiveFirstTeam, scoreForAdd.FiveSecondTeam,5) && scoreForAdd.FirstTeamSets + scoreForAdd.SecondTeamSets == 5) return BadRequest("Wrong score");
            var matchWithScore = await _repository.AddScore(scoreForAdd, id);
            var firstTeam = await _repository.GetTeam(matchToAddScore.FirstTeam.Id);
            var secondTeam = await _repository.GetTeam(matchToAddScore.SecondTeam.Id);
            await _repository.AddMatchAndRanking(firstTeam.Users, scoreForAdd.FirstTeamSets);
            await _repository.AddMatchAndRanking(secondTeam.Users, scoreForAdd.SecondTeamSets);
            if (matchToAddScore.League != null)
            {
                await _repository.AddLeagueMatchScore(matchToAddScore.League, matchToAddScore, scoreForAdd.FirstTeamSets, scoreForAdd.SecondTeamSets);
            }
            return Ok(matchWithScore);
        }
    }
}