using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models.ViewModels
{
    public class UserVacationViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public string Title { set; get; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Color { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RemainingDayOff { get; set; }
        public string GoogleCalendarId { get; set; }
    }
}
