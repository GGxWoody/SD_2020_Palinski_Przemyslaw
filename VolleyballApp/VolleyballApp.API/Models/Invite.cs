namespace VolleyballApp.API.Models
{
    public class Invite
    {
        public int Id { get; set; }
        public User InviteFrom { get; set; }
        public User InviteTo { get; set; }
        public bool FriendInvite { get; set; }
        public bool TeamInvite { get; set; }
        public bool MatchInvite { get; set; }
    }
}