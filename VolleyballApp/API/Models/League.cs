using System;
using System.Collections.Generic;

namespace VolleyballApp.API.Models
{
    public class League
    {
        public int Id { get; set; }
        public User Creator { get; set; }
        public int TeamLimit { get; set; }
        public string Description { get; set; }
        public DateTime ClosedSignUp { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<TeamLeague> TeamLeague { get; set; }
        public ICollection<Match> Matches { get; set; }
    }
}