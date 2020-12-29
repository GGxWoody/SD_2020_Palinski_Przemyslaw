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
            team.RankingPoints = team.Owner.RankingPoints;
            var userCreating = await GetUser(team.OwnerId);
            userCreating.OwnedTeam = true;
            await _context.Teams.AddAsync(team);
            _context.Users.Update(userCreating);
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
            .Include(x => x.MatchInvitedTo).OrderByDescending(x => x.Id).AsQueryable();
            invites = invites.Where(i => i.InviteTo.Id == userId);
            return await PagedList<Invite>.CreateAsync(invites, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(e => e.Team).ThenInclude(t => t.Owner)
            .Include(p => p.Photo).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photo).OrderByDescending(u => u.LastActive).AsQueryable();
            users = users.Where(u => u.Id != userParams.UserID);
            if (!String.IsNullOrEmpty(userParams.UserType) && userParams.UserType != "player")
            {
                users = users.Where(u => u.UserType == userParams.UserType);
            }
            else
            {
                users = users.Where(u => u.UserType == "player");
            }
            
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
            var matches = _context.Matches.Include(x => x.FirstTeam)
            .Include(x => x.SecondTeam).Include(x => x.Score);
            return await PagedList<Match>.CreateAsync(matches, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> IsInTeam(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user.Team != null) return true;
            return false;
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
            User userToAdd = await _context.Users.Include(x => x.Team).FirstOrDefaultAsync(x => x.Id == id);
            team.Users.Add(userToAdd);
            var rankingPointsSum = 0;
            foreach (var user in team.Users)
            {
                rankingPointsSum += user.RankingPoints;
            }
            team.RankingPoints = rankingPointsSum / team.Users.Count();
            userToAdd.Team = team;
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
            Match match = await _context.Matches.Include(x => x.League).Include(x => x.Location).Include(x => x.Score).Include(x => x.FirstTeam).ThenInclude(x => x.Owner)
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
            match.League = null;
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

        public async Task<Score> AddScore(ScoreForAddDto scoreToAdd, int id)
        {
            var match = await _context.Matches.Include(x => x.Score).FirstOrDefaultAsync(x => x.Id == id);
            var score = _context.Scores.SingleOrDefault(x => x.Id == match.Score.Id);
            score.FirstTeamSets = scoreToAdd.FirstTeamSets;
            score.SecondTeamSets = scoreToAdd.SecondTeamSets;
            score.OneFirstTeam = scoreToAdd.OneFirstTeam;
            score.TwoFirstTeam = scoreToAdd.TwoFirstTeam;
            score.ThreeFirstTeam = scoreToAdd.ThreeFirstTeam;
            score.FourFirstTeam = scoreToAdd.FourFirstTeam;
            score.FiveFirstTeam = scoreToAdd.FiveFirstTeam;
            score.OneSecondTeam = scoreToAdd.OneSecondTeam;
            score.TwoSecondTeam = scoreToAdd.TwoSecondTeam;
            score.ThreeSecondTeam = scoreToAdd.ThreeSecondTeam;
            score.FourSecondTeam = scoreToAdd.FourSecondTeam;
            score.FiveSecondTeam = scoreToAdd.FiveSecondTeam;
            await _context.SaveChangesAsync();
            return score;
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
            var invite = _context.Invites.Include(x => x.InviteFrom).Include(x => x.InviteTo).Include(x => x.MatchInvitedTo).AsQueryable();
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
            var match = await _context.Matches.Include(x => x.Location).FirstOrDefaultAsync(x => x.Id == id);
            var location = _context.Locations.SingleOrDefault(x => x.Id == match.Location.Id);
            location.Adress = locationForAdd.Adress;
            location.City = locationForAdd.City;
            location.Country = locationForAdd.Country;
            location.TimeOfMatch = locationForAdd.TimeOfMatch;
            await _context.SaveChangesAsync();

            return location;
        }

        public async Task<List<User>> AddMatchAndRanking(ICollection<User> users, int setScore)
        {
            var userList = users.ToList();
            if (setScore == 3)
            {
                userList.ForEach(a => {
                    a.GamesPlayed ++;
                    a.GamesWon ++;
                });
            } 
            else 
            {
                userList.ForEach(a => {
                    a.GamesPlayed ++;
                });
            }
            await _context.SaveChangesAsync();
                return userList;
        }

        public async Task<User> AddRefereeMatch(User user)
        {
            user.GamesPlayed ++;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<League> GetLeague(int id)
        {
            var league = await _context.Leagues.Include(x => x.Creator).Include(x => x.Matches)
            .Include(x => x.TeamLeague).ThenInclude(x => x.Team).FirstOrDefaultAsync(x => x.Id == id);
            return league;
        }

        public async Task<PagedList<League>> GetLeagues(UserParams userParams)
        {
            var leagues = _context.Leagues.Include(x => x.Creator);
            return await PagedList<League>.CreateAsync(leagues, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<League> CreateLeague(LeagueForCreationDto leagueForCreationDto)
        {
            var newLeague = new League();
            var userCreating = await GetUser(leagueForCreationDto.CreatorId);
            newLeague.Creator = userCreating;
            newLeague.City = leagueForCreationDto.City;
            newLeague.Country = leagueForCreationDto.Country;
            newLeague.TeamLimit = leagueForCreationDto.TeamLimit;
            newLeague.Description = leagueForCreationDto.Description;
            var teamLeague = new TeamLeague();
            if (userCreating.UserType == "player")
            {
                teamLeague = new TeamLeague { Team = userCreating.Team, League = newLeague };
            }
            _context.TeamLeague.Add(teamLeague);
            await _context.SaveChangesAsync();
            return teamLeague.League;
        }

        public async Task<League> AddTeamToLeague(User userJoining, League leagueToJoin)
        {
            var teamToAdd = new TeamLeague { Team = userJoining.Team, League = leagueToJoin};
            _context.TeamLeague.Add(teamToAdd);
            await _context.SaveChangesAsync();
            return leagueToJoin;
        }

        public async Task<League> CreateAndAddMatches(int leagueId)
        {
            var leagueFromRepo = await GetLeague(leagueId);
            var teamLeagues = new List<TeamLeague>(leagueFromRepo.TeamLeague);
            var teams = teamLeagues.Select(x => x.Team);
            var teamsList = new List<Team>(teams);
            for (int i = 0; i < teams.Count(); i++)
            {
                for (int j = i+1; j < teams.Count(); j++)
                {
                    await CreateLeagueMatch(teamsList[i].Id,teamsList[j].Id,leagueFromRepo.Id);
                    await CreateLeagueMatch(teamsList[j].Id,teamsList[i].Id,leagueFromRepo.Id);
                }
            }
            return leagueFromRepo;
        }

        public async Task<Match> CreateLeagueMatch(int firstTeamId, int secondTeamId, int leagueId)
        {
            Match match = new Match();
            match.FirstTeam = await GetTeam(firstTeamId);
            match.SecondTeam = await GetTeam(secondTeamId);
            match.Score = new Score();
            match.IsRefereeInvited = false;
            var leagueFromRepo = await GetLeague(leagueId);
            match.League = leagueFromRepo;
            match.Location = new Location();
            leagueFromRepo.Matches.Add(match);
            await _context.Scores.AddAsync(match.Score);
            await _context.Locations.AddAsync(match.Location);
            await _context.Matches.AddAsync(match);
            _context.Leagues.Update(leagueFromRepo);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task<PagedList<Match>> GetLeagueMatches(int leagueId, UserParams userParams)
        {
            var league = _context.Matches.Include(x => x.League).Include(x => x.FirstTeam).ThenInclude(x => x.Photo)
            .Include(x => x.SecondTeam).ThenInclude(x => x.Photo).Include(x => x.Score).Where(x => x.League.Id == leagueId);
            return await PagedList<Match>.CreateAsync(league, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<League> AddLeagueMatchScore(League league, Match match, int firstTeamScore, int secondTeamScore)
        {
            var leagueFromRepo = await GetLeague(league.Id);
            var teamLeagues = new List<TeamLeague>(leagueFromRepo.TeamLeague);
            var firstTeamLeague = teamLeagues.SingleOrDefault(x => x.Team.Id == match.FirstTeam.Id);
            var secondTeamLeague = teamLeagues.SingleOrDefault(x => x.Team.Id == match.SecondTeam.Id);
            if (firstTeamScore + secondTeamScore < 5)
            {
                if (firstTeamScore == 3)
                {
                    firstTeamLeague.LeagueGames ++;
                    firstTeamLeague.LeagueWins ++;
                    firstTeamLeague.LeagueScore += 3;
                    secondTeamLeague.LeagueLosses ++;
                    secondTeamLeague.LeagueGames ++;
                } else if (secondTeamScore == 3) 
                {
                    firstTeamLeague.LeagueGames ++;
                    firstTeamLeague.LeagueLosses ++;
                    secondTeamLeague.LeagueScore += 3;
                    secondTeamLeague.LeagueWins ++;
                    secondTeamLeague.LeagueGames ++;
                }
            } else
            {
                if (firstTeamScore == 3)
                {
                    firstTeamLeague.LeagueGames ++;
                    firstTeamLeague.LeagueWins ++;
                    firstTeamLeague.LeagueScore += 2;
                    secondTeamLeague.LeagueLosses ++;
                    secondTeamLeague.LeagueGames ++;
                    secondTeamLeague.LeagueScore ++;
                } else if (secondTeamScore == 3) 
                {
                    firstTeamLeague.LeagueGames ++;
                    firstTeamLeague.LeagueLosses ++;
                    firstTeamLeague.LeagueScore ++;
                    secondTeamLeague.LeagueScore += 2;
                    secondTeamLeague.LeagueWins ++;
                    secondTeamLeague.LeagueGames ++;
                } 
            }
            await _context.SaveChangesAsync();
            return league;
        }

        public async Task<PagedList<User>> GetRefereesFromLocation(string country, string city, UserParams userParams)
        {
            var referees = _context.Users.AsQueryable();
            referees = referees.Where(x => x.UserType == "referee");
            referees = referees.Where(x => x.City == city);
            referees = referees.Where(x => x.Country == country);
            return await PagedList<User>.CreateAsync(referees, userParams.PageNumber, userParams.PageSize);
        }
    }
}