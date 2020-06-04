
using System.Threading.Tasks;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Regiser(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}