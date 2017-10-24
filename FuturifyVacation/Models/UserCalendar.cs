using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models
{
    public class UserCalendar
    {
        [Key]
        public string CalendarId { get; set; }
        public UserProfile userProfile { get; set; }
        public int EventId { get; set; }
        public string Reason { set; get; }
        public float Duration { get; set; }
    }
}
