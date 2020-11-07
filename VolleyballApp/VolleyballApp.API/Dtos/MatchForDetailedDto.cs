using VolleyballApp.API.Models;

namespace VolleyballApp.API.Dtos
{
    public class MatchForDetailedDto
    {
        public int Id { get; set; }
        public TeamsForListDto FirstTeam { get; set; }
        public TeamsForListDto SecondTeam { get; set; }
        public int ScoreId { get; set; }
        public ScoreForSendingDto Score { get; set; }
    }
}