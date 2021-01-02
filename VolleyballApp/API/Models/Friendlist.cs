using System.Collections.Generic;

namespace VolleyballApp.API.Models
{
    public class Friendlist
    {
        public int Id { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
}