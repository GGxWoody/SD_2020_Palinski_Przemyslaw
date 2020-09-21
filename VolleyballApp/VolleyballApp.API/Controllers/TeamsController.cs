using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolleyballApp.API.Data;
using VolleyballApp.API.Dtos;
using VolleyballApp.API.Helpers;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly IVolleyballRepository _repository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public TeamsController(IVolleyballRepository repository, IMapper mapper, DataContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTeams([FromQuery]UserParams userParams){
            var teams = await _repository.GetTeams(userParams);
            var teamsToReturn = _mapper.Map<List<TeamsForListDto>>(teams);
            Response.AddPagination(teams.CurrentPage, teams.PageSize, teams.TotalCount, teams.TotalPages);
            return Ok(teamsToReturn);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(int id){
            var team = await _repository.GetTeam(id);
            var teamToReturn = _mapper.Map<TeamForDeatailedDto>(team);
            return Ok(teamToReturn);
        }

        [HttpPut("{teamId}/add/{userId}")]
        public async Task<IActionResult> AddUserToTeam(int teamId, int userId){
            if (await _repository.GetTeam(teamId) == null) return NotFound();
            if (await _repository.GetUser(userId) == null) return NotFound();
            Team team = _context.Teams.Include(e => e.Users).FirstOrDefaultAsync(u => u.Id == teamId).Result;
            User newUser = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;
            team.Users.Add(newUser);
            _context.Update(team);
            _context.SaveChanges();
            return NoContent();
        }
    }
}