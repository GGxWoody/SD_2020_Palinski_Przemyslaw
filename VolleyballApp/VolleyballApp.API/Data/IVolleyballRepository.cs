using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using VolleyballApp.API.Dtos;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Data
{
    public interface IVolleyballRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> saveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<PagedList<Team>> GetTeams(UserParams userParams);
        Task<Team> GetTeam(int id);
        Task<User> GetUser(int id);
        Task<bool> TeamExists(string name);
        Task<Team> CreateTeam(Team team);
        Task<Invite> GetFriendInvite(int userId, int id);
        Task<Invite> GetTeamInvite(int teamId, int id);
        Task<Invite> CreateFriendInvite(User sender, User reciver);
        Task<Invite> CreateTeamInvite(User recipient, Team team);
        Task<Friendlist> AcceptFriendInvite(User sender, User reciver);
        Task<Team> AcceptTeamInvite(int teamId, int id);
        Task<bool> AreFriends(int firstId, int secoundId);
        Task<bool> IsInivtedToFriends(int userId, int id);
        Task<PagedList<Invite>> GetAllUserInvites(UserParams userParams, int userId);
        Task<Invite> DeclineFriendInvite(int id, int userId);
        Task<Invite> DeclineTeamInvite(int teamId,int id);
        Task<PagedList<User>> GetFriends(UserParams userParams);
        Task<PagedList<Match>> GetMatches(UserParams userParams);
        Task<bool> IsInTeam(int teamId, int id);
        Task<bool> IsInivtedToTeam(int teamId, int id);
        Task<Match> GetMatch(int id);
        Task<bool> MatchExistsAndIsNotConcluded(int firstTeamId, int secondTeamId);
        bool TeamsShareSamePlayers(ICollection<User> firstTeamPlayers,ICollection<User> secondTeamPlayers);
        Task<Invite> GetMatchInvite(int firstTeamId, int secondTeamId);
        Task<Invite> CreateMatchInvite(Team firstTeam, Team secondTeam);
        Task<Match> AcceptMatchInvite(int firstTeamId, int secondTeamId);
        Task<bool> IsInivtedToMatch(int firstTeamId, int secondTeamId);
        Task<Invite> DeclineMatchInvite(int firstTeamId, int secondTeamId);
        Task<bool> MatchInviteExists(Team firstTeam,Team secondTeam);
        Task<Match> AddScore(ScoreForAddDto scoreToAdd,int id);
        Task<Photo> GetPhoto(int id);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
        Task<Invite> GetRefereeInvite(int matchId);
        Task<Invite> CreateRefereeInvite(User referee,Match match, int currnetUserId);
        Task<Match> AcceptRefereeInvite(int refereeId, int matchId);
        Task<Invite> DeclineRefereeInvite(int refereeId, int matchId);
        Task<Location> GetLocation(int id);
        Task<Location> AddLocation(LocationForAddDto locationForAdd, int id);
        Task<Match> AddTime(DateTime matchTime, int id);
    }
}