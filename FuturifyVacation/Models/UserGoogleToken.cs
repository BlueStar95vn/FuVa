using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models
{
    public class UserGoogleToken
    {
        [Key]
        public int Id { set; get; }
        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser User { set; get; }
        public string AccessToken { set; get; }
        public DateTime IssuedAt { set; get; }
        public string RefreshToken { get; set; }
    }
}
