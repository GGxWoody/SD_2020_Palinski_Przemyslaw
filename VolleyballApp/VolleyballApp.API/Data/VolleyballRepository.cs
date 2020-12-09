using System.Threading.Tasks;
using VolleyballApp.API.Models;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Helpers;
using System.Linq;
using System;
using AutoMapper;
using VolleyballApp.API.Dtos;
using System.Collections.Generic;

namespace VolleyballApp.API.Data
{
    public class VolleyballRepository : IVolleyballRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public VolleyballRepository(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<Team> CreateTeam(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Team> GetTeam(int id)
        {
            var team = await _context.Teams.Include(e => e.Users).ThenInclude(u => u.Photo)
            .Include(c => c.Owner).ThenInclude(u => u.Photo).Include(e => e.Photo).FirstOrDefaultAsync(u => u.Id == id);
            return team;
        }

        public async Task<PagedList<Team>> GetTeams(UserParams userParams)
        {
            var teams = _context.Teams.Include(e => e.Owner).Include(e => e.Photo).OrderByDescending(u => u.DateCreated);
            return await PagedList<Team>.CreateAsync(teams, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PagedList<Invite>> GetAllUserInvites(UserParams userParams, int userId)
        {
            var invites = _context.Invites.Include(e => e.InviteFrom).Include(e => e.InviteTo)
            .Include(x => x.TeamInvited).ThenInclude(x => x.Owner).Include(x => x.TeamInviting)
            .OrderByDescending(x => x.Id).AsQueryable();
            invites = invites.Where(i => i.InviteTo.Id == userId);
            return await PagedList<Invite>.CreateAsync(invites, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(e => e.Teams).ThenInclude(t => t.Owner)
                .Include(e => e.TeamsCreated).Include(p => p.Photo).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photo).OrderByDescending(u => u.LastActive).AsQueryable();
            users = users.Where(u => u.Id != userParams.UserID);
            users = users.Where(u => u.UserType == "player");
            if (!String.IsNullOrEmpty(userParams.Gender) && userParams.Gender != "all")
            {
                users = users.Where(u => u.Gender == userParams.Gender);
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    case "age":
                        users = users.OrderByDescending(u => u.DateOfBirth);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> saveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TeamExists(string name)
        {
            if (await _context.Teams.AnyAsync(x => x.TeamName.ToLower() == name.ToLower())) return true;
            return false;
        }

        public async Task<Invite> GetFriendInvite(int userId, int id)
        {
            var invite = await _context.Invites.Include(e => e.InviteFrom).Include(e => e.InviteTo)
            .FirstOrDefaultAsync(x => x.InviteFrom.Id == userId && x.InviteTo.Id == id && x.FriendInvite == true);
            return invite;
        }

        public async Task<Invite> CreateFriendInvite(User sender, User reciver)
        {
            Invite newInvite = new Invite();
            newInvite.InviteFrom = sender;
            newInvite.InviteTo = reciver;
            newInvite.FriendInvite = true;
            await _context.Invites.AddAsync(newInvite);
            await _context.SaveChangesAsync();
            return await GetFriendInvite(sender.Id, reciver.Id);
        }


        public async Task<Friendlist> AcceptFriendInvite(User sender, User reciver)
        {
            Friendlist fl = new Friendlist();
            fl.FirstUser = sender;
            fl.SecondUser = reciver;
            var InviteToDelete = await GetFriendInvite(sender.Id, reciver.Id);
            _context.Invites.Remove(InviteToDelete);
            await _context.Friendlist.AddAsync(fl);
            await _context.SaveChangesAsync();
            return await _context.Friendlist.Include(e => e.FirstUser).Include(e => e.SecondUser)
            .FirstOrDefaultAsync(x => x.FirstUser.Id == sender.Id && x.SecondUser.Id == reciver.Id);
        }

        public async Task<bool> AreFriends(int firstId, int secoundId)
        {
            var friendlistNode = await _context.Friendlist.FirstOrDefaultAsync(x => x.FirstUser.Id == firstId && x.SecondUser.Id == secoundId
            || x.FirstUser.Id == secoundId && x.SecondUser.Id == firstId);
            if (friendlistNode == null) return false;
            return true;
        }


        public async Task<bool> IsInivtedToFriends(int userId, int id)
        {
            var invite = await _context.Invites.FirstOrDefaultAsync(x => x.InviteFrom.Id == userId && x.InviteTo.Id == id && x.FriendInvite == true
            || x.InviteFrom.Id == id && x.InviteTo.Id == userId && x.FriendInvite == true);
            if (invite == null) return false;
            return true;
        }

        public async Task<Invite> DeclineFriendInvite(int id, int userId)
        {
            var InviteToDelete = await GetFriendInvite(id, userId);
            _context.Invites.Remove(InviteToDelete);
            await _context.SaveChangesAsync();
            return InviteToDelete;
        }

        public async Task<PagedList<User>> GetFriends(UserParams userParams)
        {
            var friendsFirstSide = _context.Friendlist.Include(x => x.FirstUser).Include(x => x.SecondUser).AsQueryable();
            friendsFirstSide = friendsFirstSide.Where(f => f.FirstUser.Id == userParams.UserID);
            var friendsSecondSide = _context.Friendlist.Include(x => x.FirstUser).Include(x => x.SecondUser).AsQueryable();
            friendsSecondSide = friendsSecondSide.Where(f => f.SecondUser.Id == userParams.UserID);
            var friends = friendsFirstSide.Select(x => x.SecondUser);
            friends.Concat(friendsSecondSide.Select(x => x.FirstUser));

            return await PagedList<User>.CreateAsync(friends, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PagedList<Match>> GetMatches(UserParams userParams)
        {
            var matches = _context.Matches.Include(x => x.FirstTeam).ThenInclude(x => x.Owner)
            .Include(x => x.SecondTeam).ThenInclude(x => x.Owner).Include(x => x.Score);
            return await PagedList<Match>.CreateAsync(matches, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> IsInTeam(int teamId, int id)
        {
            var team = await _context.Teams.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == teamId);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return team.Users.Contains(user);
        }

        public async Task<bool> IsInivtedToTeam(int teamId, int id)
        {
            var invites = _context.Invites.Include(x => x.TeamInvited).Include(x => x.InviteTo).AsQueryable();
            invites = invites.Where(x => x.TeamInvited.Id == teamId);
            invites = invites.Where(x => x.InviteTo.Id == id);
            if (await invites.FirstOrDefaultAsync() == null) return false;
            else return true;
        }

        public async Task<Invite> CreateTeamInvite(User recipient, Team team)
        {
            Invite newInvite = new Invite();
            newInvite.TeamInvited = team;
            newInvite.InviteTo = recipient;
            newInvite.TeamInvite = true;
            await _context.Invites.AddAsync(newInvite);
            await _context.SaveChangesAsync();
            return await GetTeamInvite(team.Id, recipient.Id);
        }

        public async Task<Invite> GetTeamInvite(int teamId, int id)
        {
            var invite = await _context.Invites.Include(e => e.TeamInvited)
            .Include(x => x.TeamInvited.Owner).Include(e => e.InviteTo)
            .FirstOrDefaultAsync(x => x.TeamInvited.Id == teamId && x.InviteTo.Id == id && x.TeamInvite == true);
            return invite;
        }

        public async Task<Team> AcceptTeamInvite(int teamId, int id)
        {
            Team team = await _context.Teams.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == teamId);
            User userToAdd = await _context.Users.Include(x => x.Teams).FirstOrDefaultAsync(x => x.Id == id);
            team.Users.Add(userToAdd);
            userToAdd.Teams.Add(team);
            _context.Update(team);
            _context.Update(userToAdd);
            var InviteToDelete = await GetTeamInvite(team.Id, userToAdd.Id);
            _context.Invites.Remove(InviteToDelete);
            await _context.SaveChangesAsync();
            return await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
        }

        public async Task<Invite> DeclineTeamInvite(int teamId, int id)
        {
            var InviteToDelete = await GetTeamInvite(teamId, id);
            _context.Invites.Remove(InviteToDelete);
            await _context.SaveChangesAsync();
            return InviteToDelete;
        }

        public async Task<Match> GetMatch(int id)
        {
            Match match = await _context.Matches.Include(x => x.Location).Include(x => x.Score).Include(x => x.FirstTeam).ThenInclude(x => x.Owner)
            .Include(x => x.SecondTeam).ThenInclude(x => x.Owner).Include(x => x.Referee).FirstOrDefaultAsync(x => x.Id == id);
            return match;
        }

        public async Task<bool> MatchExistsAndIsNotConcluded(int firstTeamId, int secondTeamId)
        {
            Match match = await _context.Matches.Include(x => x.Score).FirstOrDefaultAsync(x => x.FirstTeam.Id == firstTeamId && x.SecondTeam.Id == secondTeamId);
            if (match == null || match.Score.OneFirstTeam != 0 && match.Score.OneSecondTeam != 0) return false;
            return true;
        }

        public bool TeamsShareSamePlayers(ICollection<User> firstTeamPlayers, ICollection<User> secondTeamPlayers)
        {
            var sameUsers = firstTeamPlayers.Intersect(secondTeamPlayers);
            if (sameUsers.Count() == 0) return false;
            else return true;
        }

        public async Task<Invite> GetMatchInvite(int firstTeamId, int secondTeamId)
        {
            var invite = await _context.Invites.Include(e => e.TeamInvited).ThenInclude(x => x.Owner)
            .Include(e => e.TeamInviting).ThenInclude(x => x.Owner).Include(e => e.InviteTo).Include(e => e.InviteFrom)
            .FirstOrDefaultAsync(x => x.TeamInvited.Id == secondTeamId && x.TeamInviting.Id == firstTeamId && x.MatchInvite == true);
            return invite;
        }

        public async Task<Invite> CreateMatchInvite(Team firstTeam, Team secondTeam)
        {
            Invite newInvite = new Invite();
            newInvite.TeamInvited = secondTeam;
            newInvite.TeamInviting = firstTeam;
            newInvite.InviteTo = secondTeam.Owner;
            newInvite.InviteFrom = firstTeam.Owner;
            newInvite.MatchInvite = true;
            await _context.Invites.AddAsync(newInvite);
            await _context.SaveChangesAsync();
            return await GetMatchInvite(firstTeam.Id, secondTeam.Id);
        }

        public async Task<Match> AcceptMatchInvite(int firstTeamId, int secondTeamId)
        {
            Match match = new Match();
            match.FirstTeam = await GetTeam(firstTeamId);
            match.SecondTeam = await GetTeam(secondTeamId);
            match.Score = new Score();
            match.IsRefereeInvited = false;
            match.Location = new Location();
            var InviteToDelete = await GetMatchInvite(firstTeamId, secondTeamId);
            _context.Invites.Remove(InviteToDelete);
            await _context.Scores.AddAsync(match.Score);
            await _context.Locations.AddAsync(match.Location);
            await _context.Matches.AddAsync(match);
            await _context.SaveChangesAsync();
            return await _context.Matches.Include(e => e.FirstTeam).ThenInclude(e => e.Owner)
            .Include(e => e.SecondTeam).ThenInclude(e => e.Owner)
            .FirstOrDefaultAsync(x => x.FirstTeam.Id == firstTeamId && x.SecondTeam.Id == secondTeamId);
        }

        public async Task<bool> IsInivtedToMatch(int firstTeamId, int secondTeamId)
        {
            var invites = _context.Invites.Include(x => x.TeamInvited).Include(x => x.TeamInviting).AsQueryable();
            invites = invites.Where(x => x.TeamInvited.Id == secondTeamId);
            invites = invites.Where(x => x.TeamInviting.Id == firstTeamId);
            if (await invites.FirstOrDefaultAsync() == null) return false;
            else return true;
        }

        public async Task<Invite> DeclineMatchInvite(int firstTeamId, int secondTeamId)
        {
            var InviteToDelete = await GetMatchInvite(firstTeamId, secondTeamId);
            _context.Invites.Remove(InviteToDelete);
            await _context.SaveChangesAsync();
            return InviteToDelete;
        }

        public async Task<bool> MatchInviteExists(Team firstTeam, Team secondTeam)
        {
            var invite = await _context.Invites.FirstOrDefaultAsync(x => x.TeamInviting.Id == firstTeam.Id && x.TeamInvited.Id == secondTeam.Id && x.MatchInvite == true);
            if (invite == null) return false;
            return true;
        }

        public async Task<Match> AddScore(ScoreForAddDto scoreToAdd, int id)
        {
            var match = await GetMatch(id);
            var scoreToDelete = await _context.Scores.FirstOrDefaultAsync(x => x.Id == match.ScoreId);
            match.Score = _mapper.Map<Score>(scoreToAdd);
            
            _context.Matches.Update(match); 
            _context.Scores.Remove(scoreToDelete);
            if(await saveAll()) return await _context.Matches.FirstOrDefaultAsync(x => x.Id == match.Id);
            else return null;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

                public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages
            .Include(u => u.Sender).ThenInclude(p => p.Photo)
            .Include(u => u.Recipient).ThenInclude(p => p.Photo)
            .AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId
                        && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId
                        && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId
                        && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photo)
                .Include(u => u.Recipient).ThenInclude(p => p.Photo)
                 .Where(m => m.RecipientId == userId && m.RecipientDeleted == false
                     && m.SenderId == recipientId
                     || m.RecipientId == recipientId && m.SenderId == userId
                     && m.SenderDeleted == false)
                 .OrderByDescending(m => m.MessageSent)
                 .ToListAsync();

            return messages;
        }

        public async Task<Invite> GetRefereeInvite(int matchId)
        {
            var invite = _context.Invites.AsQueryable();
            invite = invite.Where(x => x.RefereeInvite == true);
            invite = invite.Where(x => x.MatchInvitedTo.Id == matchId);
            return await invite.FirstOrDefaultAsync();
        }

        public async Task<Invite> CreateRefereeInvite(User referee, Match match, int currnetUserId)
        {
            Invite newInvite = new Invite();
            newInvite.MatchInvitedTo = match;
            newInvite.InviteFrom = await GetUser(currnetUserId);
            newInvite.InviteTo = referee;
            newInvite.RefereeInvite = true;
            match.IsRefereeInvited = true;
            await _context.Invites.AddAsync(newInvite);
            _context.Update(match);
            await _context.SaveChangesAsync();
            return await GetRefereeInvite(match.Id);
        }

        public async Task<Match> AcceptRefereeInvite(int refereeId, int matchId)
        {
            Match match = await GetMatch(matchId);
            User referee = await GetUser(refereeId);
            Invite invite = await GetRefereeInvite(matchId);
            match.Referee = referee;
            if (referee.RefereeMatches == null)
            {
                referee.RefereeMatches = new List<Match>();
            }
            referee.RefereeMatches.Add(match);
            _context.Invites.Remove(invite);
            _context.Update(match);
            await _context.SaveChangesAsync();
            return await GetMatch(matchId);
        }

        public async Task<Invite> DeclineRefereeInvite(int refereeId, int matchId)
        {
            Match match = await GetMatch(matchId);
            Invite invite = await GetRefereeInvite(matchId);
            match.IsRefereeInvited = false;
            _context.Invites.Remove(invite);
            await _context.SaveChangesAsync();
            return invite;
        }

        public async Task<Location> GetLocation(int id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(x => x.Id == id);
            return location;
        }

        public async Task<Location> AddLocation(LocationForAddDto locationForAdd, int id)
        {
            var match = await GetMatch(id);
            var location = await GetLocation(match.Location.Id);
            _context.Locations.Remove(location);
            location = _mapper.Map<Location>(locationForAdd);
            match.Location = location;
            _context.Locations.Update(location);
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();

            return location;
        }

        public async Task<Match> AddTime(DateTime matchTime, int id)
        {
            var match = await GetMatch(id);
            match.TimeOfMatch = matchTime;
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();

            return await GetMatch(id);
        }
    }
}