using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models.BindingModels
{
    public class AddTeamBindingModel
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }
        public string TeamLeadId { get; set; }
    }
}
