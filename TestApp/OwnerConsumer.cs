using GraphQL.Client;
using GraphQL.Common.Request;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using TestApp.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestApp
{
    public class OwnerConsumer
    {
        private readonly GraphQLClient _client;

        public OwnerConsumer(GraphQLClient client)
        {
            _client = client;
        }

        public OwnerConsumer()
        {
        }

        public async Task<List<Appointment>> GetAppointments(DateTime start, DateTime end)
        {
            _client.DefaultRequestHeaders.Add("Authorization", "SFMyNTY.g3QAAAACZAAEZGF0YW0AAAAHNTA6MTgxOGQABnNpZ25lZG4GAHI8YElsAQ.1t0AfDf1vU1u4DV4rbv4j1VYa7WHaIw3kyHG2B8jm38");
            var query = new GraphQLRequest
            {
                Query = @"query getAppointments ($start:DateTime, $end:DateTime)
                        { 
                            appointments (updatedAtStart:$start, updatedAtEnd:$end)
                            { 
                                start 
                                insertedAt 
                                type 
                                { 
                                    id 
                                    encounterType 
                                    { 
                                        id 
                                        name 
                                    }
                                    name 
                                } 
                                status 
                                location 
                                { 
                                    name 
                                } 
                                provider 
                                { 
                                    isActive 
                                    name 
                                } 
                                patient 
                                { 
                                    clientId
                                } 
                            }
                        }",
                Variables = new { updatedAtStart = start, updatedAtEnd = end }
            };
            try
            {
                var response = await _client.PostAsync(query);
                if (response.Errors?.Any() ?? false)
                {
                    var unprocessedIds = response.Errors.Select(x => x.AdditonalEntries)
                        .Where(y => y.ContainsKey("extensions")).Where(z => z.ContainsKey("unprocessedIds"));
                }
                // to get the json string in data set
                //string jsonstr = JsonConvert.SerializeObject(response);

                List<Appointment> result = response.GetDataFieldAs<List<Appointment>>("appointments");

                //string jsonstr = JsonConvert.SerializeObject(response);

                Dictionary<TestClass, int> output = new Dictionary<TestClass, int>(new TestClass.EqualityComparer());
                HashSet<string> locationServiceSet = new HashSet<string>();
                //Dictionary<string, bool> locationServiceMap = new Dictionary<string, bool>();
                foreach (Appointment app in result)
                {
                    if (app.Start.HasValue && app.Type != null && app.Location != null)
                    {
                        //service type
                        string serviceType = app.Type.Name.ToLower().Contains("care") || app.Type.Name.ToLower().Contains("play") ? app.Type.Name : "Style";

                        DateTime currDate = app.Start.Value.Date;
                        DateTime lastDate = currDate.AddDays(14);
                        DateTime firstDate = currDate.AddDays(-28);

                        for (DateTime x = firstDate; x < lastDate; x = x.AddDays(1))
                        {
                            TestClass xTestClass = new TestClass(x, serviceType, app.Location.Name);
                            if (!output.ContainsKey(xTestClass))
                                output.Add(xTestClass, 0);

                            TestClass totalAppClass = new TestClass(x, "Total No of Appointments", app.Location.Name);
                            if (!output.ContainsKey(totalAppClass))
                                output.Add(totalAppClass, 0);

                            if (serviceType.ToLower().Contains("care"))
                            {
                                TestClass totalCare = new TestClass(x, "Total Care", app.Location.Name);
                                if (!output.ContainsKey(totalCare))
                                    output.Add(totalCare, 0);
                            }
                        }

                        // increment by 1 for all three case below
                        TestClass test = new TestClass(currDate, serviceType, app.Location.Name);
                        output[test] = output[test] + 1;

                        // Aggregation - Total Care
                        if (serviceType.ToLower().Contains("care"))
                        {
                            TestClass totalCare = new TestClass(app.Start.Value.Date, "Total Care", app.Location.Name);
                            output[totalCare] = output[totalCare] + 1;
                        }

                        // Aggregation - Total Appointments
                        TestClass totalAppointments = new TestClass(app.Start.Value.Date, "Total No of Appointments", app.Location.Name);
                        output[totalAppointments] = output[totalAppointments] + 1;

                    }
                }

                //var output1 = output.GroupBy(x => new { x.Key.Location }, pair => pair.Value).ToDictionary(g => g.Key, g =>g.ToList());
                WriteToExcel(output);

                Debug.WriteLine(output.Count);
                foreach (KeyValuePair<TestClass, int> kvp in output.OrderBy(x => x.Key.Location).ThenBy(x => x.Key.Type).ThenBy(x => x.Key.Start))
                {
                    Debug.WriteLine("{0}, -> {1}", kvp.Key, output[kvp.Key].ToString(), kvp.Value);
                }



                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteToExcel(Dictionary<TestClass, int> dict)
        {
            Excel.Application oXL = new Excel.Application();
            Excel._Workbook oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            Excel._Worksheet oSheet = (Excel._Worksheet)oWB.ActiveSheet;
            Excel.Range oRng;

            string[] saNames = { "Test1", "Test2"};

            oRng = oSheet.get_Range("A1", Missing.Value).get_Resize(1, dict.Keys.Count);
            oRng.Value = saNames;

            oXL.Visible = true;
            oXL.UserControl = true;
        }
    }
}