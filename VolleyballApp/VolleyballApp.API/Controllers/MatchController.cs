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
    }
}