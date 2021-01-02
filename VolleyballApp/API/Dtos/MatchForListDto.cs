namespace VolleyballApp.API.Dtos
{
    public class MatchForListDto
    {
        public int Id { get; set; }
        public TeamsForListDto FirstTeam { get; set; }
        public TeamsForListDto SecondTeam { get; set; }
        public ScoreForListDto Score { get; set; }
    }
}