﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Models.BindingModels
{
    public class DayOffConfigBindingModel
    {
        public int NumberOfDayOff { get; set; }
        public int HoursADay { get; set; }
        public int DurationInMonth { get; set; }
    }
}
