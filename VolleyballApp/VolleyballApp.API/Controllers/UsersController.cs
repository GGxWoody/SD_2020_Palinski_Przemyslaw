using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using VolleyballApp.API.Data;
using VolleyballApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DatingApp.API.Helpers;
using System;
using VolleyballApp.API.Helpers;

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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currnetUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repository.GetUser(currnetUserId);
            userParams.UserID = currnetUserId;
            if (String.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "male" : "female";
            }
            var users = await _repository.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name= "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var currnetUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _repository.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            userToReturn.IsFriend = await _repository.AreFriends(currnetUserId, id);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var userFromRepo = await _repository.GetUser(id);
            _mapper.Map(userForUpdateDto, userFromRepo);
            if (await _repository.saveAll()) return NoContent();
            throw new Exception($"Updating user with {id} failed on save.");
        }
    }
}