using VolleyballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace VolleyballApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Friendlist> Friendlist { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Set> Sets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Team>()
            .HasOne(t => t.Owner)
            .WithMany(u => u.TeamsCreated)
            .HasForeignKey(t => t.OwnerId);
        }
    }
}