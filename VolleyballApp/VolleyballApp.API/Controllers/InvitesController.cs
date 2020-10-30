using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using VolleyballApp.API.Data;
using VolleyballApp.API.Dtos;
using VolleyballApp.API.Helpers;

namespace VolleyballApp.API.Controllers
{
    [Route("api/invites/{userId}")]
    [ApiController]
    public class InvitesController : ControllerBase
    {
        private readonly IVolleyballRepository _repository;
        private readonly IMapper _mapper;

        public InvitesController(IVolleyballRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetFriendInvites([FromQuery]UserParams userParams, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var invites = await _repository.GetAllUserFriendInvites(userParams ,userId);
            var invitesToReturn = _mapper.Map<List<InviteToReturnDto>>(invites);
            Response.AddPagination(invites.CurrentPage, invites.PageSize, invites.TotalCount, invites.TotalPages);
            return Ok(invitesToReturn);
        }

        [HttpGet("friend/{id}", Name = "GetInvite")]
        public async Task<IActionResult> GetFriendInvite(int userId, int id)
        {
            if (userId == id) return BadRequest();
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
                && id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var inviteFromRepo = await _repository.GetFriendInvite(userId, id);
            if (inviteFromRepo == null)
                return NotFound();

            var inviteToDisplay = _mapper.Map<InviteToReturnDto>(inviteFromRepo);
            return Ok(inviteToDisplay);
        }

        [HttpPut("friend/{id}")]
        public async Task<IActionResult> AcceptFriendInvite(int userId, int id)
        {
            if (userId == id) return BadRequest();
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            if (await _repository.GetFriendInvite(userId, id) == null)
                return NotFound();
            var inviteFromRepo = await _repository.GetFriendInvite(userId, id);
            var sender = await _repository.GetUser(userId);
            var recipient = await _repository.GetUser(id);
            if (sender == null || recipient == null) return NotFound();
            var friendFromRepo = await _repository.AcceptFriendInvite(sender,recipient);
            var inviteToDisplay = _mapper.Map<FriendToReturnDto>(friendFromRepo);

            return Ok(inviteToDisplay);
        }

        [HttpPost("friend/{id}")]
        public async Task<IActionResult> SendFriendInvite(int id, int userId)
        {
            if (userId == id) return BadRequest();
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            if (await _repository.AreFriends(userId,id)) return BadRequest("Already friends");
            if (await _repository.IsInivtedToFriends(userId,id)) return BadRequest("Already invited");
            var user = await _repository.GetUser(userId);
            var recipient = await _repository.GetUser(id);
            if (recipient == null) return BadRequest("Could not find user");
            var invite = await _repository.CreateFriendInvite(user,recipient);
            var inviteToReturn = _mapper.Map<InviteToReturnDto>(invite);
            return CreatedAtRoute("GetInvite", new { userId = user.Id, id = recipient.Id}, inviteToReturn);
        }
    }
}