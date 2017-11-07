using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string UserId { get; set; }
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
        public bool HasPassword { set; get; } 
    }
}
