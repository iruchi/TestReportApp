using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using AppointmentReport.Models;

namespace AppointmentReport.Controllers
{
    [RoutePrefix("report")]
    public class ReportController : BaseController
    {
        public ReportController()
        {
        }

        [Route("apt")]
        [HttpPost]
        public void Post()
        {
            DateTime currentWeekMonday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            DateTime currentWeekSunday = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
            DateTime firstDate = currentWeekMonday.AddDays(-28);
            DateTime endDate = currentWeekSunday.AddDays(14);

            IEnumerable<string> res = Get(firstDate, endDate);
            //var owners = _consumer.GetAppointments(firstDate, endDate);
            //return Ok(owners);
        }

        [HttpGet]
        public IEnumerable<string> Get(DateTime firstDate, DateTime endDate)
        {
            //DateTime firstDate = DateTime.Now.AddDays(-28), lastDate = DateTime.Now.AddDays(14);
            var client = new MongoClient("mongodb+srv://TJDev_hp:Heartandpawvathtj123@chameleoncluster-f9wmz.mongodb.net/HPReport_Dev?retryWrites=true&w=majority");
            var db = client.GetDatabase("HPReport_Dev");

            var collection = db.GetCollection<BsonDocument>("HPReport_Appointments_Dev");

            var aggregate = collection.Aggregate()
                .Project(new BsonDocument
                {
                    { "id", 1 },
                    { "insertedAt", 1 },
                    { "patient", 1 },
                    { "Location", 1 },
                    { "type", 1 },
                    { "provider", 1 },
                    { "start", 1 },
                    { "Status", 1 },
                    { "StartDate", new BsonDocument("$dateToString", new BsonDocument("format", "%Y-%m-%d")
                                        .Add("date", new BsonDocument("$add", new BsonArray(new object[] { "$start", 5 * 60 * 60 * 1000 }))))
                    }
                })
               .Match(new BsonDocument("StartDate", new BsonDocument("$gte", firstDate.ToString("yyyy-MM-dd")).Add("$lte", endDate.ToString("yyyy-MM-dd"))))
               .Project(new BsonDocument
                {
                    { "id", 1 },
                    { "insertedAt", 1 },
                    { "patient", 1 },
                    { "Location", 1 },
                    { "type", 1 },
                    { "provider", 1 },
                    { "start", 1 },
                    { "Status", 1 }
                });

            var results = aggregate.ToList();
            List<Appointment> result = new List<Appointment>();
            foreach (var obj in results)
            {
                //obj.Remove("_id");
                var appt = BsonSerializer.Deserialize<Appointment>(obj);
                result.Add(appt);
            }
            int count = result.Count();

            return new string[] { "value1", "value2" };
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