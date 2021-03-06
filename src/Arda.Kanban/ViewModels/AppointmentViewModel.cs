﻿using System;

namespace Arda.Kanban.ViewModels
{
    public class AppointmentViewModel
    {
        public Guid _AppointmentID { get; set; }

        public string _AppointmentComment { get; set; }

        public DateTime _AppointmentDate { get; set; }

        public int _AppointmentHoursDispensed { get; set; }

        public decimal _AppointmentTE { get; set; }

        public string _AppointmentUserUniqueName { get; set; }

        public Guid _AppointmentWorkloadWBID { get; set; }

        // Additional information

        public string _AppointmentUserName { get; set; }

        public string _WorkloadTitle { get; set; }
    }
}
