using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
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
        Task<Invite> CreateFriendInvite(User sender, User reciver);
        Task<Friendlist> AcceptFriendInvite(User sender, User reciver);
        Task<bool> AreFriends(int firstId, int secoundId);
        Task<bool> IsInivtedToFriends(int userId, int id);
        Task<PagedList<Invite>> GetAllUserFriendInvites(UserParams userParams, int userId);
    }
}