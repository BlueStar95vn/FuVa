using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models.ViewModels
{
    public class ChangePassViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
