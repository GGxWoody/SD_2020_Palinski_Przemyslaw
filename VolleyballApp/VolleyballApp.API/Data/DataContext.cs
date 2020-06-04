using VolleyballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace VolleyballApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
        public DbSet<User> Users { get; set; }
    }
}