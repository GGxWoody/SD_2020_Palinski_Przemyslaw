using System.Collections.Generic;

namespace VolleyballApp.API.Dtos
{
    public class LeagueForDetailedDto
    {
        public int Id { get; set; }
        public UserForListDto Creator { get; set; }
        public int TeamLimit { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<TeamLeagueForLeagueDetailsDto> TeamLeague { get; set; }
    }
}