using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApp.Models
{
    public class TestClass
    {
        public TestClass(DateTime start, string type, string location)
        {
            Start = start.ToString("yyyy/MM/dd");
            Type = type;
            Location = location;
        }

        //public override bool Equals(Object obj)
        //{
        //    //Check for null and compare run-time types.
        //    if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        TestClass p = (TestClass)obj;
        //        return (Start == p.Start) && (Type == p.Type) && (Location == p.Location);
        //    }
        //}

        //public override int GetHashCode()
        //{
        //    return (x << 2) ^ y;
        //}

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", Location, Type, Start);
        }

        public string Start { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }

        public class EqualityComparer : IEqualityComparer<TestClass>
        {
            public bool Equals(TestClass x, TestClass y)
            {
                return x.Start.ToLower().Equals(y.Start.ToLower()) && x.Type.ToLower().Equals(y.Type.ToLower()) && x.Location.ToLower().Equals(y.Location.ToLower());
            }

            public int GetHashCode(TestClass x)
            {
                string combined = x.Start + "|" + x.Type + "|" + x.Location;
                return combined.GetHashCode();

            }
        }
    }

    //public class MyClassSpecialComparer : IEqualityComparer<TestClass>
    //{
    //    public bool Equals(TestClass x, TestClass y)
    //    {
    //        return x.Start == y.Start && x.Type == y.Type && x.Location == y.Location;
    //    }

    //    public int GetHashCode(TestClass x)
    //    {
    //        return x.Start.GetHashCode() + x.Type.GetHashCode() + x.Location.GetHashCode();
    //    }
}