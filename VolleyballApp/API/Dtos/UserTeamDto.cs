namespace VolleyballApp.API.Dtos
{
    public class UserTeamDto
    {
        
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public UserForListDto User { get; set; }
        public TeamsForListDto Team { get; set; }
        public bool IsTeamOwner { get; set; }
    }
}