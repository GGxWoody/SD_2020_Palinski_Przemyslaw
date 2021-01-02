using System.ComponentModel.DataAnnotations.Schema;

namespace VolleyballApp.API.Models
{
    public class UserTeam
    {
        [Column(Order = 0)]
        public int UserId { get; set; }
        [Column(Order = 1)]
        public int TeamId { get; set; }
        public User User { get; set; }
        public Team Team { get; set; }
        public bool IsTeamOwner { get; set; }
    }
}