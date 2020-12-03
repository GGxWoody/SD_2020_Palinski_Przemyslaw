using System;

namespace VolleyballApp.API.Dtos
{
    public class TeamsForListDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public DateTime DateCreated { get; set; }
        public UserForTeamListDto Owner { get; set; }
        public string Description { get; set; }
    }
}