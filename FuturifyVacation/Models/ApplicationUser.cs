using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UserProfile UserProfile { get; set; }
       public List<TeamDetail> TeamDetail { get; set; }
    }
}
