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

namespace VolleyballApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/invites/{userId}")]
    [Authorize]
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
        public async Task<IActionResult> GetUserInvites([FromQuery]UserParams userParams, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var invites = await _repository.GetAllUserInvites(userParams ,userId);
            var invitesToReturn = _mapper.Map<List<InviteToReturnDto>>(invites);
            Response.AddPagination(invites.CurrentPage, invites.PageSize, invites.TotalCount, invites.TotalPages);
            return Ok(invitesToReturn);
        }

        [HttpGet("friend/{id}", Name = "GetFriendInvite")]
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
            Helpers.MailSender.sendInviteAction("friend", "accepted", sender.Mail);
            return Ok(inviteToDisplay);
        }

        [HttpDelete("friend/{id}")]
        public async Task<IActionResult> DeclineFriendInvite(int id, int userId)
        {
            if (userId == id) return BadRequest();
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var sender = await _repository.GetUser(userId);
            if (await _repository.IsInivtedToFriends(userId,id))
            {
                await _repository.DeclineFriendInvite(userId, id);
                Helpers.MailSender.sendInviteAction("friend", "declined", sender.Mail);
                return NoContent();
            }
            return BadRequest("No existing invite from this user");
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
            if (recipient.IsMailActivated == false) return BadRequest("User account is not activated");
            var invite = await _repository.CreateFriendInvite(user,recipient);
            var inviteToReturn = _mapper.Map<InviteToReturnDto>(invite);
            Helpers.MailSender.sendInviteInfo("friend", recipient.Mail);
            return CreatedAtRoute("GetFriendInvite", new { userId = user.Id, id = recipient.Id}, inviteToReturn);
        }

        [HttpPost("team/{teamId}/{id}")]
        public async Task<IActionResult> SendTeamInvite(int id, int teamId, int userId)
        {
            if (userId == id) return BadRequest();
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            if (await _repository.IsInTeam(id)) return BadRequest("Already in team");
            if (await _repository.IsInivtedToTeam(teamId,id)) return BadRequest("Already invited");
            if (!await _repository.AreFriends(userId,id)) return BadRequest("Must be friends first");
            var team = await _repository.GetTeam(teamId);
            if (team == null) return BadRequest("Could not find team");
            if (team.OwnerId != userId) return BadRequest("Must be owner to invite to team");
            var recipient = await _repository.GetUser(id);
            var sender = await _repository.GetUser(userId);
            if (recipient.UserType != "player") return BadRequest("You cannot invite this user to team");
            if (recipient.Team != null) return BadRequest("This user is already in some team");
            if (recipient.IsMailActivated == false) return BadRequest("User account is not activated");
            if (team.Id != sender.Team.Id) return BadRequest("You are not part of that team");
            if (sender.OwnedTeam == false) return BadRequest("You are not owner of that team");
            if (recipient == null) return BadRequest("Could not find user");
            var invite = await _repository.CreateTeamInvite(recipient,team);
            var inviteToReturn = _mapper.Map<InviteToReturnDto>(invite);
            Helpers.MailSender.sendInviteInfo("team", recipient.Mail);
            return CreatedAtRoute("GetTeamInvite", new {userId = userId, teamId = team.Id, id = recipient.Id}, inviteToReturn);
        }

        [HttpGet("team/{teamId}/{id}", Name = "GetTeamInvite")]
        public async Task<IActionResult> GetTeamInvite(int id, int teamId, int userId)
        {
            if (userId == id) return BadRequest();
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
                && id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var inviteFromRepo = await _repository.GetTeamInvite(teamId, id);
            if (inviteFromRepo == null)
                return NotFound();

            var inviteToDisplay = _mapper.Map<InviteToReturnDto>(inviteFromRepo);
            return Ok(inviteToDisplay);
        }

        [HttpPut("team/{teamId}/{id}")]
        public async Task<IActionResult> AcceptTeamInvite(int id, int teamId, int userId)
        {
            if (userId == id) return BadRequest();
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            if (await _repository.GetTeamInvite(teamId, id) == null)
                return NotFound();
            var team = await _repository.GetTeam(teamId);
            var recipient = await _repository.GetUser(id);
            if (team == null || recipient == null) return NotFound();
            var teamFromRepo = await _repository.AcceptTeamInvite(teamId, id);
            var teamToDisplay = _mapper.Map<TeamForDeatailedDto>(teamFromRepo);
            Helpers.MailSender.sendInviteAction("team", "accepted", teamFromRepo.Owner.Mail);

            return Ok(teamToDisplay);
        }

        [HttpDelete("team/{teamId}/{id}")]
        public async Task<IActionResult> DeclineTeamInvite(int id, int teamId, int userId)
        {
            if (userId == id) return BadRequest();
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var teamFromRepo = await _repository.GetTeam(teamId);
            if (await _repository.IsInivtedToTeam(teamId,id))
            {
                await _repository.DeclineTeamInvite(teamId, id);
                Helpers.MailSender.sendInviteAction("team", "declined", teamFromRepo.Owner.Mail);
                return NoContent();
            }
            return BadRequest("No existing invite for this user");
        }

        [HttpGet("match/{firstTeamId}/{secondTeamId}", Name = "GetMatchInvite")]
        public async Task<IActionResult> GetMatchInvite(int firstTeamId, int secondTeamId, int userId)
        {
            var firstTeam = await _repository.GetTeam(firstTeamId);
            var secondTeam = await _repository.GetTeam(secondTeamId);
            if (firstTeam.OwnerId == secondTeam.OwnerId) return BadRequest();
            if (firstTeam.OwnerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
                && secondTeam.OwnerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var inviteFromRepo = await _repository.GetMatchInvite(firstTeamId, secondTeamId);
            if (inviteFromRepo == null)
                return NotFound();

            var inviteToDisplay = _mapper.Map<InviteToReturnDto>(inviteFromRepo);
            return Ok(inviteToDisplay);
        }

        [HttpPost("match/{firstTeamId}/{secondTeamId}")]
        public async Task<IActionResult> SendMatchInvite(int firstTeamId, int secondTeamId, int userId)
        {
            var firstTeam = await _repository.GetTeam(firstTeamId);
            var secondTeam = await _repository.GetTeam(secondTeamId);
            var currnetUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);


            if (firstTeam == null || secondTeam == null) BadRequest("One of the team does not exist");
            var firstTeamPlayers = firstTeam.Users;
            var secondTeamPlayers = secondTeam.Users;


            if (currnetUserId != firstTeam.OwnerId) return BadRequest("You have to be owner of team to send invite for match");
            if (firstTeam.Owner.Id == secondTeam.Owner.Id) return BadRequest("Teams owned by the same person");
            if (await _repository.MatchExistsAndIsNotConcluded(firstTeam.Id, secondTeam.Id)) return BadRequest("Match exists and has not been concluded");
            if (_repository.TeamsShareSamePlayers(firstTeamPlayers, secondTeamPlayers)) return BadRequest("Match cannot be created if team share players");
            if (await _repository.MatchInviteExists(firstTeam, secondTeam)) return BadRequest("Invite was already send and awaits acceptation");

            var invite = await _repository.CreateMatchInvite(firstTeam,secondTeam);
            var inviteToReturn = _mapper.Map<InviteToReturnDto>(invite);
            Helpers.MailSender.sendInviteInfo("match", secondTeam.Owner.Mail);
            return CreatedAtRoute("GetMatchInvite", new {firstTeamId = firstTeamId, secondTeamId = secondTeamId, userId = firstTeam.OwnerId}, inviteToReturn);

        }

        [HttpPut("match/{firstTeamId}/{secondTeamId}")]
        public async Task<IActionResult> AcceptMatchInvite(int firstTeamId, int secondTeamId, int userId)
        {
            var firstTeam = await _repository.GetTeam(firstTeamId);
            var secondTeam = await _repository.GetTeam(secondTeamId);
            if (firstTeam == null || secondTeam == null)
                return NotFound();
            if (secondTeam.OwnerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            if (await _repository.GetMatchInvite(firstTeamId, secondTeamId) == null)
                return NotFound();
            var matchFromRepo = await _repository.AcceptMatchInvite(firstTeamId, secondTeamId);
            var matchToDisplay = _mapper.Map<MatchForDetailedDto>(matchFromRepo);
            Helpers.MailSender.sendInviteAction("match", "accepted", firstTeam.Owner.Mail);
            return Ok(matchToDisplay);
        }

        [HttpDelete("match/{firstTeamId}/{secondTeamId}")]
        public async Task<IActionResult> DeclineMatchInvite(int firstTeamId, int secondTeamId, int userId)
        {
            var firstTeam = await _repository.GetTeam(firstTeamId);
            var secondTeam = await _repository.GetTeam(secondTeamId);
            if (firstTeam == null || secondTeam == null)
                return NotFound();
            if (secondTeam.OwnerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            if (await _repository.IsInivtedToMatch(firstTeamId, secondTeamId))
            {
                await _repository.DeclineMatchInvite(firstTeamId, secondTeamId);
                Helpers.MailSender.sendInviteAction("match", "declined", firstTeam.Owner.Mail);
                return NoContent();
            }
            return BadRequest("No existing invite for this user");
        }


        [HttpGet("match/{matchId}/{refereeId}/referee", Name = "GetRefereeInvite")]
        public async Task<IActionResult> GetRefereeInvite(int userId, int matchId, int refereeId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var inviteFromRepo = await _repository.GetRefereeInvite(matchId);
            if (inviteFromRepo == null) return NotFound();

            var inviteToDisplay = _mapper.Map<InviteToReturnDto>(inviteFromRepo);
            return Ok(inviteToDisplay);
        }

        [HttpPost("match/{matchId}/{refereeId}/referee")]
        public async Task<IActionResult> SendRefereeInvite(int matchId, int refereeId)
        {
            var referee = await _repository.GetUser(refereeId);
            if (referee == null) return BadRequest ("User does not exist");
            if (referee.UserType != "referee") return BadRequest ("User is not a referee");
            var match = await _repository.GetMatch(matchId);
            if (match == null) return BadRequest ("Match does not exist");
            if (match.IsRefereeInvited == true) return BadRequest ("Referee is already invited for this match cancel it or wait for referee to accept");
            var currnetUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currnetUserId != match.FirstTeam.OwnerId && currnetUserId != match.SecondTeam.OwnerId ) return BadRequest("You have to be owner of team taking part in a match to send invite refree to join match");

            var invite = await _repository.CreateRefereeInvite(referee, match, currnetUserId);
            var inviteToReturn = _mapper.Map<InviteToReturnDto>(invite);
            Helpers.MailSender.sendInviteInfo("match", referee.Mail);
            return CreatedAtRoute("GetRefereeInvite", new {userId = currnetUserId, matchId = matchId, refereeId = refereeId}, inviteToReturn);

        }

        [HttpPut("match/{matchId}/referee")]
        public async Task<IActionResult> AcceptRefereeInvite(int matchId)
        {
            var currnetUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var invite = await _repository.GetRefereeInvite(matchId);
            if (invite == null) return BadRequest("This invitation does not exists");
            if (currnetUserId != invite.InviteTo.Id) BadRequest("This is not your invitation");
            
            var matchFromRepo = await _repository.AcceptRefereeInvite(invite.InviteTo.Id, matchId);
            var matchToDisplay = _mapper.Map<MatchForDetailedDto>(matchFromRepo);
            Helpers.MailSender.sendInviteAction("referee", "accepted", invite.InviteFrom.Mail);
            return Ok(matchToDisplay);
        }

        [HttpDelete("match/{matchId}/referee")]
        public async Task<IActionResult> DeclineRefereeInvite(int matchId)
        {
            var currnetUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var invite = await _repository.GetRefereeInvite(matchId);
            if (invite == null) return BadRequest("This invitation does not exists");
            if (invite.InviteTo.Id != currnetUserId && invite.InviteFrom.Id != currnetUserId) return BadRequest("This is not your invite");
            await _repository.DeclineRefereeInvite(currnetUserId, matchId);
            Helpers.MailSender.sendInviteAction("referee", "declined", invite.InviteFrom.Mail);
            return NoContent();
        }

    }
}