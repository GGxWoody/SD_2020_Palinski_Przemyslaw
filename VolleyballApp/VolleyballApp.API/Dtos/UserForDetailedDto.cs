using System;
using System.Collections.Generic;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public PhotoForReturnDto Photo { get; set; }
        public int GamesLost { get; set; }
        public int GamesWon { get; set; }
        public ICollection<TeamsForListDto> Teams { get; set; }
        public ICollection<TeamsForListDto> TeamsCreated { get; set; }
        public bool IsFriend { get; set; }
    }
}