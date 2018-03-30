using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrishRailTimetables
{
    //Created using http://json2csharp.com/
    public class Result
    {
        public Result ShowResults;
        public string arrivaldatetime { get; set; }
        public string duetime { get; set; }
        public string departuredatetime { get; set; }
        public string departureduetime { get; set; }
        public string scheduledarrivaldatetime { get; set; }
        public string scheduleddeparturedatetime { get; set; }
        public string destination { get; set; }
        public string destinationlocalized { get; set; }
        public string origin { get; set; }
        public string originlocalized { get; set; }
        public string direction { get; set; }
        public string @operator { get; set; }
        public string additionalinformation { get; set; }
        public string lowfloorstatus { get; set; }
        public string route { get; set; }
        public string sourcetimestamp { get; set; }
        public string monitored { get; set; }


        public override string ToString()
        {
            return string.Format("\nDeparture From: {0}\t\tArriving To: {1}\nDeparture Date/Time: {2}\tArrival Date/Time: {3}\n\n",
                origin, destination, departuredatetime, scheduledarrivaldatetime);
        }


    }
}
