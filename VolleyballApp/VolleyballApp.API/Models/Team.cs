using System;
using System.Collections.Generic;

namespace VolleyballApp.API.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<User> Users { get; set; }
        public DateTime DateCreated { get; set; }
    }
}