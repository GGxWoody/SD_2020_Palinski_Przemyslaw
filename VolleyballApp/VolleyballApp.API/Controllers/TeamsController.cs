using System;
using System.Collections.Generic;
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
            
            if(userFromRepo.UserType != "player") return BadRequest("Only players can create team");
            if(userFromRepo.Team != null) return BadRequest("You are already in the team");

            teamToCreate.Owner = userFromRepo;
            teamToCreate.OwnerId = userFromRepo.Id;
            teamToCreate.DateCreated = System.DateTime.Now;
            teamToCreate.Users = new List<User>();
            teamToCreate.Users.Add(userFromRepo);

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
    }
}