namespace VolleyballApp.API.Dtos
{
    public class LeagueForListDto
    {
        public int Id { get; set; }
        public UserForListDto Creator { get; set; }
        public int TeamLimit { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}