using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class TeamsController : ControllerBase
    {
        private readonly IVolleyballRepository _repository;
        private readonly IMapper _mapper;

        public TeamsController(IVolleyballRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetTeams([FromQuery]UserParams userParams){
            var teams = await _repository.GetTeams(userParams);
            var teamsToReturn = _mapper.Map<List<TeamsForListDto>>(teams);
            Response.AddPagination(teams.CurrentPage, teams.PageSize, teams.TotalCount, teams.TotalPages);
            return Ok(teamsToReturn);
        }
        [HttpGet("{id}", Name= "GetTeam")]
        public async Task<IActionResult> GetTeam(int id){
            var team = await _repository.GetTeam(id);
            var teamToReturn = _mapper.Map<TeamForDeatailedDto>(team);
            if (team.Photo != null)
            {
                teamToReturn.PhotoUrl = team.Photo.Url;
            }
            return Ok(teamToReturn);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewTeam(TeamForCreationDto teamForCreationDto)
        {
            if(await _repository.TeamExists(teamForCreationDto.TeamName)) return BadRequest("Team with that name already exists");

            var teamToCreate = _mapper.Map<Team>(teamForCreationDto);

            var currnetUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repository.GetUser(currnetUserId);
            if (userFromRepo.IsMailActivated == false) return BadRequest("User account is not activated");
            
            if(userFromRepo.UserType != "player") return BadRequest("Only players can create team");
            if(userFromRepo.UserTeam != null) return BadRequest("You are already in the team");

            teamToCreate.Owner = userFromRepo;
            teamToCreate.OwnerId = userFromRepo.Id;
            teamToCreate.DateCreated = System.DateTime.Now;
            teamToCreate.UserTeams = new List<UserTeam>();
            UserTeam newUser = new UserTeam();
            newUser.Team = teamToCreate;
            newUser.User = userFromRepo;
            newUser.IsTeamOwner = true;
            teamToCreate.UserTeams.Add(newUser);

            var teamCreated = await _repository.CreateTeam(teamToCreate);

            var teamToReturn = _mapper.Map<TeamForDeatailedDto>(teamCreated);

            return CreatedAtRoute("GetTeam", new { controller = "Teams", id = teamCreated.Id }, teamToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, TeamForUpdateDto teamForUpdateDto)
        {
            var teamFromRepo = await _repository.GetTeam(id);
            if (teamFromRepo.OwnerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            _mapper.Map(teamForUpdateDto, teamFromRepo);
            if (await _repository.saveAll()) return NoContent();
            throw new Exception($"Updating team with {id} failed on save.");
        }

        [HttpDelete("{id}/{userId}")]
        public async Task<IActionResult> RemoveUserFromTeam(int id, int userId)
        {
            var team = await _repository.GetTeam(id);
            if (team == null) return BadRequest("This team does not exist");
            if (team.OwnerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            if (!team.UserTeams.Select(x => x.UserId).Contains(userId)) return BadRequest("This player is not in your team");
            return Ok(await _repository.RemoveUserFromTeam(id, userId));
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> LeaveTeam(int userId)
        {
            var user = await _repository.GetUser(userId);
            if (user.UserTeam == null) return BadRequest("User is not in any team");
            if (user.UserTeam.IsTeamOwner) return BadRequest("Owner can't leave team");
            return Ok(await _repository.RemoveUserFromTeam(user.UserTeam.TeamId, userId));
        }
    }
}