using System;

namespace VolleyballApp.API.Dtos
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public TeamsForListDto Team { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int GamesPlayed { get; set; }
        public string PhotoUrl { get; set; }
        public int RankingPoints { get; set; }
        public bool OwnedTeam { get; set; }
        public bool IsMailActivated { get; set; }
        public string Mail { get; set; }
    }
}