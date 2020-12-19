namespace VolleyballApp.API.Dtos
{
    public class LeagueForCreationDto
    {
        public int CreatorId { get; set; }
        public int TeamLimit { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}