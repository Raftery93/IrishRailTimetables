using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrishRailTimetables
{
    //Created using http://json2csharp.com/
    //Contains getters & setters for api object
    public class ResultLocation
    {
        public ResultLocation ShowLocationResults;
        public string stopid { get; set; }
        public string displaystopid { get; set; }
        public string shortname { get; set; }
        public string shortnamelocalized { get; set; }
        public string fullname { get; set; }
        public string fullnamelocalized { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string lastupdated { get; set; }
        public List<LocationOperator> operators { get; set; }

        //Only return certain items
        public override string ToString()
        {
            return string.Format("Latitude: {0}\tLongitude: {1}\n",
                latitude, longitude);
        }
    } 
}
