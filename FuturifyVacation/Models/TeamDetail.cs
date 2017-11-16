using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models
{
    public class TeamDetail
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public UserTeam Team { get; set; }

        [ForeignKey("TeamMember")]
        public string UserId { get; set; }
        public ApplicationUser TeamMember { get; set; }
        public string RoleInTeam { get; set; }
    }
}
