using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models
{
    public class UserProfile
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public ApplicationUser User { get; set; }
        public string Email { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }      
        public string Position { set; get; }
        public string Department { set; get; }
        public DateTime DoB { set; get; }
        public string PhoneNumber { set; get; }
        public string Gender { set; get; }
        public string RemainingDayOff { set; get; }
        public string Status { set; get; }

    }
}
