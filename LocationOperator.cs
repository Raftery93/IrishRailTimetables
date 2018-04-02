using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrishRailTimetables
{
    //Created using http://json2csharp.com/
    //Contains getters & setters for api object
    public class LocationOperator
    {
        public string name { get; set; }
        public List<string> routes { get; set; }

        public override string ToString()
        {
            // Returns vars from Results class
            return string.Format("{0}", string.Join("", routes));
        }
    }
}
