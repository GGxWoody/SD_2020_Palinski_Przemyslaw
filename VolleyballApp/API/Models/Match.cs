using System;

namespace VolleyballApp.API.Models
{
    public class Match
    {
        public int Id { get; set; }
        public Team FirstTeam { get; set; }
        public Team SecondTeam { get; set; }
        public int ScoreId { get; set; }
        public Score Score { get; set; }
        public User Referee { get; set; }
        public bool IsRefereeInvited { get; set; }
        public Location Location { get; set; }
        public League League { get; set; }
    }
}