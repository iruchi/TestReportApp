using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReport.Models
{
    public enum AppointmentStatus
    {
        ADMITTED
        ,CANCELLED
        ,CHECKED_IN
        ,CHECKING_OUT
        ,COMPLETED
        ,CONFIRMED
        ,DAYCARE
        ,INPROGRESS
        ,MEDBOARD
        ,NOSHOW
        ,PENDING
        ,PLANNED
    }
}