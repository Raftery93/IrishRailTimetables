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

    /* Conor Raftery G00274094
    * This class contains details about geoLocation, users location, and manipulating all of those details
    */
    class MyLocation
    {
        //Variables that are called in the MainPage class
        public static string latitude = "Empty";
        public static string longitude = "Empty";
        public static string fullDetails2 = "";
        public static string originTitle = "";
        public static double[] distances = new double[28];

        //Used for getting/setting longitude & latitude
        public Geoposition Geopositionpos { get; private set; }

        //Used for choosing what format for distance
        public enum DistanceType { Miles, Kilometers };

        //Basic struct for a position
        public struct Position
        {
            public double Latitude;
            public double Longitude;
        }

        //Used for calculation
        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }

        //https://en.wikipedia.org/wiki/Haversine_formula
        //Used Haversine formula to get distance between 2 points
        public double Distance(Position pos1, Position pos2, DistanceType type)
        {
            //Selecting miles/km
            double R = (type == DistanceType.Miles) ? 3960 : 6371;
            //Get distance - latitude
            double dLat = this.toRadian(pos2.Latitude - pos1.Latitude);
            //Get distance - longitude
            double dLon = this.toRadian(pos2.Longitude - pos1.Longitude);
            //Adapted from formula: hav(c) = hav(a-b) + sin(a)sin(b) hav(C)
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(this.toRadian(pos1.Latitude)) * Math.Cos(this.toRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            //return distance
            return d;
        }

        //This method is called from the MainPage which is used to get the user location.
        //Lines 79-109, 117-130, 190-202 explained in MainPage.callForLocation_Click
        public async void getLocation_Click(object sender, RoutedEventArgs e)
        {
            //Gets users location and places it into longitude & latitude
            var geoLocator = new Geolocator();
            geoLocator.DesiredAccuracy = PositionAccuracy.High;
            Geopositionpos = await geoLocator.GetGeopositionAsync();
            latitude = Geopositionpos.Coordinate.Point.Position.Latitude.ToString();
            longitude = Geopositionpos.Coordinate.Point.Position.Longitude.ToString();

            string baseUrl = "https://data.smartdublin.ie/cgi-bin/rtpi/busstopinformation?stopid=";
            string endUrl = "&format=json";
            string stationSelected = "SLIGO";

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

            string[] stationCodes = new string[] {"BFSTC", "SLIGO", "DDALK","WPORT", "DGHDA","RSCMN","ATLNE","MYNTH",
                 "CNLLY","HSTON","ATHRY", "ORNMR","GALWY","KDARE","PTLSE","CRLOW","ENNIS","THRLS","LMRCK","KKNNY",
                "LMRKJ","TIPRY","CVILL","WXFRD","TRLEE","MLLOW", "CORK", "COBH"};

            double[] latAllStations = new double[28];
            double[] lonAllStations = new double[28];
            string[] originAllStation = new string[28];

            //Loops to keep creating new url with specific stationCode
            for (int i = 0; i < stationCodes.Length; i++)
            {
                //Get each stationCode via loop, and places it into string/url
                string stationSelectedforCo = stationCodes[i];

                client = new HttpClient();

                client.BaseAddress = new Uri(baseUrl + stationSelectedforCo + endUrl);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = client.GetAsync(baseUrl + stationSelectedforCo + endUrl).Result;

                result = response.Content.ReadAsStringAsync().Result;

                obj = JsonConvert.DeserializeObject<RootLocation>(result);

                latStation = obj.results[0].latitude;
                lonStation = obj.results[0].longitude;
                //Gets the station name depending on which station the loop is on
                originTitle = obj.results[0].displaystopid;

                doubleLatStation = double.Parse(latStation);
                doubleLonStation = double.Parse(lonStation);

                //Places longitude, latitude & station name into variables which are passed(called) back to MainPage
                //which in turn are displayed in xaml
                latAllStations[i] = doubleLatStation;
                lonAllStations[i] = doubleLonStation;
                originAllStation[i] = originTitle;
            }

            //Parses latitude & longitude strings to doubles
            double doubleLat = double.Parse(latitude);
            double doubleLon = double.Parse(longitude);

            //Generates 2 positions (for distance checking)
            Position pos1 = new Position();
            Position pos2 = new Position();

            //Put user coords in pos1
            pos1.Latitude = doubleLat;
            pos1.Longitude = doubleLon;

            //Put station coords in pos2
            pos2.Latitude = latAllStations[0];
            pos2.Longitude = lonAllStations[0];

            //Initiate variables
            double smallestDistance;
            int smallestIndex =0;

            //Loop to get distance from all stations
            for (int i = 0; i < stationCodes.Length; i++)
            {
                pos2.Latitude = latAllStations[i];
                pos2.Longitude = lonAllStations[i];

                //Populate array of distances via distance from each station and the users location
               distances[i] = Distance(pos1, pos2, DistanceType.Kilometers);
            }

            //Set the shortest distance to index 0 in distance array
            smallestDistance = distances[0];

            //Loop through all distances
            for (int i = 0; i < stationCodes.Length; i++)
            {
                //Get shortest distance
                if (distances[i]<smallestDistance)
                {
                    //Set variables for shortest distance
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
        }
    }
}