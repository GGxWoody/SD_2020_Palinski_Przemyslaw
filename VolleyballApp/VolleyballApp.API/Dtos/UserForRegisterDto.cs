using System;
using System.ComponentModel.DataAnnotations;

namespace VolleyballApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 4,ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }

        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string UserType { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Mail { get; set; }

        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}