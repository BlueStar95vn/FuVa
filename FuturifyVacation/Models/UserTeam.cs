using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models
{
    public class UserTeam
    {       
        [Key]
       public int Id { get; set; }
        public string TeamName { get; set; }

        [ForeignKey("Profile")]
        public string TeamLeadId { get; set; }
        
        public UserProfile Profile { get; set; }
    }   
}
