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
    }
}