using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrishRailTimetables
{
    class DisplayItems
    {
        //public string destinationStation { get; set; }
       // public string originStation { get; set; }
       // public string departureTime { get; set; }
       // public string arrivalTime { get; set; }

        public int ID
        {
            get;
            set;
        }

        public string stationName
        {
            get;
            set;

        }

        public override string ToString()
        {
            return this.stationName;
        }
    }


}
