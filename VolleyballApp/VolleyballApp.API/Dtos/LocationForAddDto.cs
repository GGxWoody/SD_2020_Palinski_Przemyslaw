using System;

namespace VolleyballApp.API.Dtos
{
    public class LocationForAddDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public DateTime TimeOfMatch { get; set; }
    }
}