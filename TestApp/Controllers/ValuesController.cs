using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;


namespace TestApp.Controllers
{
    [RoutePrefix("values")]
    public class ValuesController : BaseController
    {
        //private readonly OwnerConsumer _consumer;


        public ValuesController()
        {
            //_consumer = new OwnerConsumer();
        }

        [Route("apt")]
        [HttpPost]
        public void Post()
        {
            DateTime currentWeekMonday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            DateTime currentWeekSunday = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
            DateTime firstDate = currentWeekMonday.AddDays(-28);
            DateTime endDate = currentWeekSunday.AddDays(14);

            //var owners = _consumer.GetAppointments(firstDate, endDate);
            //return Ok(owners);
        }
    }

    static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7; return dt.AddDays(-1 * diff).Date;
        }
    }
}