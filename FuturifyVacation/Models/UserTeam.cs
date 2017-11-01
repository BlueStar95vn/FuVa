using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models
{
    public class UserTeam
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }
        public int NumberOfMembers { get; set; }
        public string TeamLeadId { get; set; }
        
    }
}
