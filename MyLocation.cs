using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IrishRailTimetables
{

    class MyLocation
    {
        public static string latitude = "Empty";
        public static string longitude = "Empty";
        public static string fullDetails2 = "";
        public static string originTitle = "";



        public Geoposition Geopositionpos { get; private set; }

        public async void getLocation_Click(object sender, RoutedEventArgs e)
        {
            var geoLocator = new Geolocator();
            geoLocator.DesiredAccuracy = PositionAccuracy.High;
            Geopositionpos = await geoLocator.GetGeopositionAsync();
            latitude = Geopositionpos.Coordinate.Point.Position.Latitude.ToString();
            longitude = Geopositionpos.Coordinate.Point.Position.Longitude.ToString();


            //Debug.WriteLine("Latitude: " + latitude);
            //Debug.WriteLine("\nLongitude: " + longitude);

            /////////////////////////////////////////////////////////////////////////////////////
            //https://data.smartdublin.ie/cgi-bin/rtpi/busstopinformation?stopid=SLIGO&format=json

            string baseUrl = "https://data.smartdublin.ie/cgi-bin/rtpi/busstopinformation?stopid=";
            string endUrl = "&format=json";
            string stationSelected = "SLIGO";

            //Result Class
            //Root Object Class

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(baseUrl + stationSelected + endUrl);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(baseUrl + stationSelected + endUrl).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            
            var obj = JsonConvert.DeserializeObject<RootLocation>(result);

            string fullDetails = obj.ToString();

            string latStation = obj.results[0].latitude;
            string lonStation = obj.results[0].longitude;

            double doubleLatStation = double.Parse(latStation);
            double doubleLonStation = double.Parse(lonStation);



            Debug.WriteLine(doubleLatStation);
            //Debug.WriteLine("fullLocation: " + lonStation + "\n" + "fullLocation: " + latStation + "\n");


            Debug.WriteLine("fullLocation: " + fullDetails+ "\n");

            ///////////////////////////////////////
            

            string[] stationCodes = new string[] {"BFSTC", "SLIGO", "DDALK","WPORT", "DGHDA","RSCMN","ATLNE","MYNTH","CNLLY","HSTON","ATHRY",
                "ORNMR","GALWY","KDARE","PTLSE","CRLOW","ENNIS","THRLS","LMRCK","KKNNY","LMRKJ","TIPRY","CVILL","WXFRD","TRLEE","MLLOW", "CORK", "COBH"};

            double[] latAllStations = new double[28];
            double[] lonAllStations = new double[28];
            string[] originAllStation = new string[28];

            for (int i = 0; i < stationCodes.Length; i++)
            {
                string stationSelectedforCo = stationCodes[i];

                client = new HttpClient();

                client.BaseAddress = new Uri(baseUrl + stationSelectedforCo + endUrl);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = client.GetAsync(baseUrl + stationSelectedforCo + endUrl).Result;

                result = response.Content.ReadAsStringAsync().Result;


                obj = JsonConvert.DeserializeObject<RootLocation>(result);


                latStation = obj.results[0].latitude;
                lonStation = obj.results[0].longitude;
                originTitle = obj.results[0].displaystopid;
                //here

                doubleLatStation = double.Parse(latStation);
                doubleLonStation = double.Parse(lonStation);


                //These are mixed up
                latAllStations[i] = doubleLatStation;
                lonAllStations[i] = doubleLonStation;
                originAllStation[i] = originTitle;
            }


            for (int i = 0; i < stationCodes.Length; i++)
            {
                Debug.WriteLine("\n");
                Debug.WriteLine("Station Name: " + originAllStation[i]);
                Debug.WriteLine("Station code: "+stationCodes[i]);//Contains all station codes in order via index
                Debug.WriteLine("Latitude: "+latAllStations[i]);//Contains all latitude coords in order via index
                Debug.WriteLine("Longitude: "+lonAllStations[i]);//Contains all  longitude coords in order via index
                Debug.WriteLine("\n");
            }

            //Devices coords
            Debug.WriteLine("Latitude: " + latitude);
            Debug.WriteLine("\nLongitude: " + longitude);

            double doubleLat = double.Parse(latitude);
            double doubleLon = double.Parse(longitude);

            //Compare my device coords against stations coords to find nearest
            //https://stackoverflow.com/questions/12835851/find-closest-location-with-longitude-and-latitude
            //https://en.wikipedia.org/wiki/Haversine_formula

            //USE THE FOLLOWING LINK, CHECK DISTANCE BETWEEN YOU AND EVERY STATION, FIND THE ONE WITH THE LEAST DISTANCE
            //https://stackoverflow.com/questions/28569246/how-to-get-distance-between-two-locations-in-windows-phone-8-1

            Position pos1 = new Position();
            Position pos2 = new Position();

            pos1.Latitude = doubleLat;
            pos1.Longitude = doubleLon;

            pos2.Latitude = latAllStations[0];
            pos2.Longitude = lonAllStations[0];

            double[] distances = new double[28];
            double smallestDistance;
            int smallestIndex =0;

            for (int i = 0; i < stationCodes.Length; i++)
            {
                pos2.Latitude = latAllStations[i];
                pos2.Longitude = lonAllStations[i];

               distances[i] = Distance(pos1, pos2, DistanceType.Kilometers);
            }



            for (int i = 0; i < stationCodes.Length; i++)
            {
                Debug.WriteLine("SmallestDistance: " + distances[i] + "\n");
            }

            smallestDistance = distances[0];

            for (int i = 0; i < stationCodes.Length; i++)
            {
                if (distances[i]<smallestDistance)
                {
                    originTitle = originAllStation[i];
                    smallestDistance = distances[i];
                    smallestIndex = i;
                }
            }

            string baseUrl2 = "https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=";
            string stationSelected2 = stationCodes[smallestIndex];
            string endUrl2 = "&format=json";

            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri(baseUrl2 + stationSelected2 + endUrl2);
            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response2 = client2.GetAsync(baseUrl2 + stationSelected2 + endUrl2).Result;
            var result2 = response2.Content.ReadAsStringAsync().Result;

            var obj2 = JsonConvert.DeserializeObject<RootObject>(result2);

            fullDetails2 = obj2.ToString();
            //originTitle = obj2.results[smallestIndex].origin;

            Debug.WriteLine("Closest station details: " + fullDetails2);

        }

        public enum DistanceType { Miles, Kilometers };

        public struct Position
        {
            public double Latitude;
            public double Longitude;
        }

        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }

        public double Distance(Position pos1, Position pos2, DistanceType type)
        {
            double R = (type == DistanceType.Miles) ? 3960 : 6371;
            double dLat = this.toRadian(pos2.Latitude - pos1.Latitude);
            double dLon = this.toRadian(pos2.Longitude - pos1.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(this.toRadian(pos1.Latitude)) * Math.Cos(this.toRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d;
        }




    }

}


/*
 * https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/geocoding
 * 
 * use this to add get your location placename
 */
