using System.Collections.Generic;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Dtos
{
    public class ScoreForSendingDto
    {
        public int Id { get; set; }
        public int FirstTeamSets { get; set; }
        public int SecondTeamSets { get; set; }
        public ICollection<Set> SetList { get; set; }
    }
}