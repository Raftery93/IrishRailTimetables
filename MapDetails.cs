using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Maps;

namespace IrishRailTimetables
{
    class MapDetails
    {
        public Geoposition Geopositionpos { get; private set; }

        public void AddSpaceNeedleIcon()
        {
            var MyLandmarks = new List<MapElement>();

            BasicGeoposition snPosition = new BasicGeoposition { Latitude = 47.620, Longitude = -122.349 };
            Geopoint snPoint = new Geopoint(snPosition);

            var spaceNeedleIcon = new MapIcon
            {
                Location = snPoint,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                ZIndex = 0,
                Title = "Space Needle"
            };

            MyLandmarks.Add(spaceNeedleIcon);

            var LandmarksLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = MyLandmarks
            };

            MainPage.MapControl2.Layers.Add(LandmarksLayer);

            MainPage.MapControl2.Center = snPoint;
            MainPage.MapControl2.ZoomLevel = 14;

        }

        public async void getLongitude()
        {
            var geoLocator = new Geolocator();
            geoLocator.DesiredAccuracy = PositionAccuracy.High;
            Geopositionpos = await geoLocator.GetGeopositionAsync();
            string longitude = Geopositionpos.Coordinate.Point.Position.Longitude.ToString();
            string latitude = Geopositionpos.Coordinate.Point.Position.Latitude.ToString();
        }
        
    }
}
