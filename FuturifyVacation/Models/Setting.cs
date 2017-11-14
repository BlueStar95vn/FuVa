using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public int NumberOfDayOff { get; set; }
        public int DurationInWeek { get; set; }
        public int DurationInMonth { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
    }
}
