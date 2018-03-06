using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrishRailTimetables
{
    //Created using http://json2csharp.com/
    public class RootObject
    {
        public string errorcode { get; set; }
        public string errormessage { get; set; }
        public int numberofresults { get; set; }
        public string stopid { get; set; }
        public string timestamp { get; set; }
        public List<Result> results { get; set; }


        public override string ToString()
        {
            // Returns vars from Results class
            return string.Format("{0}", string.Join("", results));
        }

    }
}
