using System;
using System.Collections.Generic;

namespace VolleyballApp.API.Dtos
{
    public class TeamForDeatailedDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int OwnerId { get; set; }
        public UserForListDto Owner { get; set; }
        public ICollection<UserForListDto> Users { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
    }
}