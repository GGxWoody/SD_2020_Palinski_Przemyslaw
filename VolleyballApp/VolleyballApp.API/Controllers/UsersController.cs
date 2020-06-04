using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using VolleyballApp.API.Data;
using VolleyballApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VolleyballApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IVolleyballRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IVolleyballRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repository.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repository.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);


            return Ok(userToReturn);
        }
    }
}