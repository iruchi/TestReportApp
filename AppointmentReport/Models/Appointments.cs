using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReport.Models
{
    public class Appointment
    {
        public DateTime? Start { get; set; }
        public DateTime? InsertedAt { get; set; }
        public AppointmentType Type { get; set; }
        public string Status { get; set; }
        public Location Location { get; set; }
        public Provider Provider { get; set; }
        public Patient Patient { get; set; }
    }
}