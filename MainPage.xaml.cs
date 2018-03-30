using Newtonsoft.Json;
using System;
using Windows.Storage;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IrishRailTimetables
{

    public sealed partial class MainPage : Page
    {

   

        List<DisplayItems> sNames = new List<DisplayItems>();
        int selectIndex;

        string[] stationCodes = new string[] {"BFSTC", "SLIGO", "DDALK","WPORT", "DGHDA","RSCMN","ATLNE","MYNTH","CNLLY","HSTON","ATHRY",
                "ORNMR","GALWY","KDARE","PTLSE","CRLOW","ENNIS","THRLS","LMRCK","KKNNY","LMRKJ","TIPRY","CVILL","WXFRD","TRLEE","MLLOW", "CORK", "COBH"};

        string[] stationNames = new string[] {"Belfast (Central)", "Sligo", "Dundalk","Westport", "Drogheda","Roscommon","Athlone","Maynooth","Dublin (Connolly)","Dublin (Heuston)","Athenry",
                "Oranmore","Galway","Kildare","Portlaoise","Carlow","Ennis","Thurles","Limerick","Kilkenny","Limerick (Junction)","Tipperary","Charleville","Wexford","Tralee","Mallow", "Cork", "Cobh"};

        public static MapControl MapControl2 = new MapControl();


        public MainPage()
        {
            this.InitializeComponent();

            // Add the MapControl and the specify maps authentication key.

            // MapControl2.ZoomInteractionMode = MapInteractionMode.GestureAndControl;
            // MapControl2.TiltInteractionMode = MapInteractionMode.GestureAndControl;
            // MapControl2.MapServiceToken = "4eHjP7348hDlfxfexoHq~b19fzIoJ9f9tKXS6zjtgzQ~AkcMoHnOY06TTP7cZlx7HU6p51K8tdzCES0s6AfLdGfIzxYbOiXZybD_DiYZ_nd0";
            //pageGrid.Children.Add(MapControl2);

            //MapDetails md = new MapDetails();

            //md.AddSpaceNeedleIcon();
            string baseUrl = "https://data.smartdublin.ie/cgi-bin/rtpi/busstopinformation?stopid=";
            string endUrl = "&format=json";
            string stationSelected = "SLIGO";

            double[] latAllStations = new double[28];
            double[] lonAllStations = new double[28];


            for (int i = 0; i < stationCodes.Length; i++)
            {
                string stationSelectedforCo = stationCodes[i];

                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(baseUrl + stationSelectedforCo + endUrl);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(baseUrl + stationSelectedforCo + endUrl).Result;

                var result = response.Content.ReadAsStringAsync().Result;


                var obj = JsonConvert.DeserializeObject<RootLocation>(result);


                string latStation = obj.results[0].latitude;
                string lonStation = obj.results[0].longitude;
                //here

               double doubleLatStation = double.Parse(latStation);
               double doubleLonStation = double.Parse(lonStation);


                //These are mixed up
                latAllStations[i] = doubleLatStation;
                lonAllStations[i] = doubleLonStation;
            }

            for (int i = 0; i < stationCodes.Length; i++)
            {
                Debug.WriteLine("\nIn MainPage");
                Debug.WriteLine("Station code: " + stationCodes[i]);//Contains all station codes in order via index
                Debug.WriteLine("Latitude: " + latAllStations[i]);//Contains all latitude coords in order via index
                Debug.WriteLine("Longitude: " + lonAllStations[i]);//Contains all  longitude coords in order via index
                Debug.WriteLine("\n");
            }


            var MyLandmarks = new List<MapElement>();

            BasicGeoposition snPosition = new BasicGeoposition { Latitude = 53.4239, Longitude = -7.9407 };
            Geopoint snPoint = new Geopoint(snPosition);



            var LandmarksLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = MyLandmarks
            };

            MapControl1.Layers.Add(LandmarksLayer);

            MapControl1.Center = snPoint;
            MapControl1.ZoomLevel = 8;

            //=======================

            PushPin pushPin = new PushPin();

            
            


            //for (int i = 0; i < pushPin.Items().Count; i++)
           for (int i = 0; i < lonAllStations.Length; i++)
              {
                pushPin.AddPushPin(latAllStations[i], lonAllStations[i]);
                MapIcon myIcon = new MapIcon();
                myIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                myIcon.Title = stationNames[i] + " Station";
                MapControl1.MapElements.Add(myIcon);
                myIcon.Location = pushPin.MyGeopoint(i);
            }



            //Populates ComboBox
            for (int i = 0; i<stationNames.Length;i++) {
                sNames.Add(new DisplayItems()
                {
                    ID = i,
                    stationName = stationNames[i]
                });
            }

        }//MainPage

        internal class PushPin

        {

            private ObservableCollection<Geopoint> items;

            public PushPin()
            {
                items = new ObservableCollection<Geopoint>();
            }

            public void AddPushPin(double latitude, double longitude)
            {
                items.Add(new Geopoint(new BasicGeoposition() { Latitude = latitude, Longitude = longitude }));
            }

            public Geopoint MyGeopoint(int i)
            {
                return items[i];
            }

            public ObservableCollection<Geopoint> Items()
            {
                return items;
            }
        }




        //public static TextBlock LatitudeTextBox;
        //public static TextBlock LongitudeTextBox;

        void callForLocation_Click(object sender, RoutedEventArgs e)
        {
            MyLocation location = new MyLocation();
            location.getLocation_Click(sender, e);


            string latitude = MyLocation.latitude;
            string longitude = MyLocation.longitude;
            string details = MyLocation.fullDetails2;
            string titleOrigin = MyLocation.originTitle;


            if (latitude == "Empty")
            {
                LatitudeTextBox.Text = "\tAn error occured, please wait 15 seconds and try again";

            }
            else
            {

                LatitudeTextBox.Text = "\t"+latitude+"\n";
                LongitudeTextBox.Text = "\t" + longitude + "\n";
                detailsTextBox.Text = details;
                originTitleTextBox.Text = "\t"+titleOrigin+"\n";
            }



        }


        void select_OnClick(object sender, RoutedEventArgs e)
        {

            selectIndex = ComboxBox1.SelectedIndex;
            object currentText = ComboxBox1.SelectedValue;
            Debug.WriteLine(currentText);

            if (selectIndex == -1)
            {
                stationDetailsTextBox.Text = "Please select a station";
            }
            else
            {

                string stationSelected = stationCodes[selectIndex];

                //Adapted from https://stackoverflow.com/questions/36516146/parsing-json-in-uwp

                string baseUrl = "https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=";
                string endUrl = "&format=json";


                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl + stationSelected + endUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(baseUrl + stationSelected + endUrl).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                var obj = JsonConvert.DeserializeObject<RootObject>(result);

                string fullDetails = obj.ToString();

                Debug.WriteLine("fullDetails: " + fullDetails);

                //string destinationStation = obj.results[0].destination;
                //string originStation = obj.results[0].origin;
                //string departureTime = obj.results[0].scheduleddeparturedatetime;
                //string arrivalTime=obj.results[0].scheduledarrivaldatetime;


                if (fullDetails != "")
                {

                    stationDetailsTextBox.Text = fullDetails;
                }
                else
                {
                    stationDetailsTextBox.Text = "No current trains leaving from that station right now";
                }

                //Remove switch statement and return selectIndex. Compare it against the string
                //array of stationCodes. Add stationCodes[selectIndex] to url string. Output fullDetails
                //to TextBox using stationDetailsTextBox.Text = fullDetails;
            }
            
        }





        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            try
            {
                pvtTabs.SelectedIndex = (int)localSettings.Values["currentTab"];
                preferedStation.SelectedText = (string)localSettings.Values["prefStation"];

            }
            catch
            {
                pvtTabs.SelectedIndex = 1;
                preferedStation.SelectedText = "";
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void pvtTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            localSettings.Values["currentTab"] = pvtTabs.SelectedIndex;
        }

        private void preferedStation_Changed(object sender, TextChangedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            localSettings.Values["prefStation"] = preferedStation.Text;

        }


    }





}
