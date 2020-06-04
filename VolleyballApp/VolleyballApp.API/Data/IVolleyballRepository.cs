using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Data
{
    public interface IVolleyballRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> saveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
    }
}