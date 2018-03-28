using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

namespace IrishRailTimetables
{

    class MyLocation
    {

        public Geoposition Geopositionpos { get; private set; }

        private async void getLocation_Click(object sender, RoutedEventArgs e)
        {
            var geoLocator = new Geolocator();
            geoLocator.DesiredAccuracy = PositionAccuracy.High;
            Geopositionpos = await geoLocator.GetGeopositionAsync();
            string latitude = "Latitude: " + Geopositionpos.Coordinate.Point.Position.Latitude.ToString();
            string longitude = "Longitude: " + Geopositionpos.Coordinate.Point.Position.Longitude.ToString();

            Debug.WriteLine("Latitude: " + latitude);
            Debug.WriteLine("Longitude: " + longitude);
        }

    }

}
