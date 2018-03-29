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
        public TextBlock txtRefL;

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

            double intLatStation = double.Parse(latStation);
            double intLonStation = double.Parse(lonStation);



            Debug.WriteLine(intLatStation);
            //Debug.WriteLine("fullLocation: " + lonStation + "\n" + "fullLocation: " + latStation + "\n");


            Debug.WriteLine("fullLocation: " + fullDetails+ "\n");

            ///////////////////////////////////////
            

            string[] stationCodes = new string[] {"BFSTC", "SLIGO", "DDALK","WPORT", "DGHDA","RSCMN","ATLNE","MYNTH","CNLLY","HSTON","ATHRY",
                "ORNMR","GALWY","KDARE","PTLSE","CRLOW","ENNIS","THRLS","LMRCK","KKNNY","LMRKJ","TIPRY","CVILL","WXFRD","TRLEE","MLLOW", "CORK", "COBH"};

            double[] latAllStations = new double[28];
            double[] lonAllStations = new double[28];

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

                intLatStation = double.Parse(latStation);
                intLonStation = double.Parse(lonStation);



                latAllStations[i] = intLatStation;
                lonAllStations[i] =intLonStation;
            }


            for (int i = 0; i < stationCodes.Length; i++)
            {
                Debug.WriteLine("\n");
                Debug.WriteLine("Station code: "+stationCodes[i]);//Contains all station codes in order via index
                Debug.WriteLine("Latitude: "+latAllStations[i]);//Contains all latitude coords in order via index
                Debug.WriteLine("Longitude: "+lonAllStations[i]);//Contains all  longitude coords in order via index
                Debug.WriteLine("\n");
            }

            //Devices coords
            Debug.WriteLine("Latitude: " + latitude);
            Debug.WriteLine("\nLongitude: " + longitude);

            //Compare my device coords against stations coords to find nearest
            //https://stackoverflow.com/questions/12835851/find-closest-location-with-longitude-and-latitude
            //https://en.wikipedia.org/wiki/Haversine_formula

            //USE THE FOLLOWING LINK, CHECK DISTANCE BETWEEN YOU AND EVERY STATION, FIND THE ONE WITH THE LEAST DISTANCE
            //https://stackoverflow.com/questions/28569246/how-to-get-distance-between-two-locations-in-windows-phone-8-1




        }




    }

}
