using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrishRailTimetables
{
    class DisplayItems : INotifyPropertyChanged
    {
        //public string destinationStation { get; set; }
        // public string originStation { get; set; }
        // public string departureTime { get; set; }
        // public string arrivalTime { get; set; }
        private string selectedStation;

        public event PropertyChangedEventHandler PropertyChanged;

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

        /*
        public string SelectedStation
        {
            set
            {
                selectedStation = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("stationName"));
                }
            }
            get
            {
                return selectedStation;
            }
        }
        */

        public override string ToString()
        {
            return this.stationName;
        }
    }


}
