﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models
{
    public class UserVacation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        public UserProfile User { get; set; }
        //public ApplicationUser User { get; set; }
        
        public string Title { set; get; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Color { get; set; }
        public int Hours { get; set; }
        public string GoogleCalendarId { get; set; }

    }
}
