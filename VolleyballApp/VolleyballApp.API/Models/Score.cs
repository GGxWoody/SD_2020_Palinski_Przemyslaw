using System.Collections.Generic;

namespace VolleyballApp.API.Models
{
    public class Score
    {
        public int Id { get; set; }
        public Match Match { get; set; }
        public int FirstTeamSets { get; set; }
        public int SecondTeamSets { get; set; }
        public int OneFirstTeam { get; set; }
        public int OneSecondTeam { get; set; }
        public int TwoFirstTeam { get; set; }
        public int TwoSecondTeam { get; set; }
        public int ThreeFirstTeam { get; set; }
        public int ThreeSecondTeam { get; set; }
        public int FourFirstTeam { get; set; }
        public int FourSecondTeam { get; set; }
        public int FiveFirstTeam { get; set; }
        public int FiveSecondTeam { get; set; }

    }
}