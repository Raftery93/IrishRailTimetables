using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrishRailTimetables
{
    public class RootLocation
    {
        public string errorcode { get; set; }
        public string errormessage { get; set; }
        public int numberofresults { get; set; }
        public string timestamp { get; set; }
        public List<ResultLocation> results { get; set; }


        public override string ToString()
        {
            // Returns vars from Results class
            return string.Format("{0}", string.Join("", results));
        }
    }
}
