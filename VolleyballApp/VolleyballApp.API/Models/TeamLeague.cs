using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolleyballApp.API.Models
{
    public class TeamLeague
    {
    [Column(Order = 0)]
    public int LeagueId { get; set; }
    [Column(Order = 1)]
    public int TeamId { get; set; }
    public League League { get; set; }
    public Team Team { get; set; }

    public int LeagueScore { get; set; }
    public int LeagueGames { get; set; }
    public int LeagueWins{ get; set; }
    public int LeagueLosses { get; set; }
    }
}