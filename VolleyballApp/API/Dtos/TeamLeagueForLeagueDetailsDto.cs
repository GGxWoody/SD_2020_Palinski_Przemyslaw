namespace VolleyballApp.API.Dtos
{
    public class TeamLeagueForLeagueDetailsDto
    {
    public int LeagueId { get; set; }
    public int TeamId { get; set; }
    public TeamsForListDto Team { get; set; }
    public int LeagueScore { get; set; }
    public int LeagueGames { get; set; }
    public int LeagueWins{ get; set; }
    public int LeagueLosses { get; set; }
    }
}