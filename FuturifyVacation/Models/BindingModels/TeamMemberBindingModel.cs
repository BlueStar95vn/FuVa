using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models.BindingModels
{
    public class TeamMemberBindingModel
    {
        public int TeamId { get; set; }       
        public string FirstName { get; set; }
        public string LastName { get; set; }      
        public string RoleInTeam { get; set; }
        public string UserId { get; set; }
    }
}
