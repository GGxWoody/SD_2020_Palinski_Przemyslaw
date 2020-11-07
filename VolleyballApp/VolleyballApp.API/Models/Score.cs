using System.Collections.Generic;

namespace VolleyballApp.API.Models
{
    public class Score
    {
        public int Id { get; set; }
        public Match Match { get; set; }
        public int FirstTeamSets { get; set; }
        public int SecondTeamSets { get; set; }
        public ICollection<Set> SetList { get; set; }
    }
}