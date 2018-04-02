using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrishRailTimetables
{
    //Contains getters & setters for populating combobox
    class DisplayItems
    {
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
