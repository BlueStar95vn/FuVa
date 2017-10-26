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
        public ApplicationUser User { get; set; }
        public string Title { set; get; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
