namespace VolleyballApp.API.Dtos
{
    public class FriendToReturnDto
    {
        public int Id { get; set; }
        public UserForListDto FirstUser { get; set; }
        public UserForListDto SecoundUser { get; set; }
    }
}