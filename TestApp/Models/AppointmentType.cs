using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApp.Models
{
    public class AppointmentType
    {
        public string Id { get; set; }
        public EncounterType EncounterType { get; set; }
        public string Name { get; set; }
    }
}