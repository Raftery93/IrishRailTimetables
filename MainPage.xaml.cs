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
using Windows.Media.Playback;
using Windows.Media.Core;

/* Conor Raftery G00274094
 * This class contains all the details that is pushed to Mainpage.xaml
*/

namespace IrishRailTimetables
{
    public sealed partial class MainPage : Page
    {
        //Creates a list of DisplayItems for combobox
        List<DisplayItems> sNames = new List<DisplayItems>();

        //Used for combobox selection
        int selectIndex;

        //Creates a string array of station codes used for referencing against the json api
        string[] stationCodes = new string[] {"BFSTC", "SLIGO", "DDALK","WPORT", "DGHDA","RSCMN","ATLNE","MYNTH",
            "CNLLY","HSTON","ATHRY","ORNMR","GALWY","KDARE","PTLSE","CRLOW","ENNIS","THRLS","LMRCK","KKNNY",
            "LMRKJ","TIPRY","CVILL","WXFRD","TRLEE","MLLOW", "CORK", "COBH"};

        //Creates a string array of station names for displaying in xaml
        string[] stationNames = new string[] {"Belfast (Central)", "Sligo", "Dundalk","Westport", "Drogheda",
            "Roscommon","Athlone","Maynooth","Dublin (Connolly)","Dublin (Heuston)","Athenry", "Oranmore",
            "Galway","Kildare","Portlaoise","Carlow","Ennis","Thurles","Limerick","Kilkenny","Limerick (Junction)",
            "Tipperary","Charleville","Wexford","Tralee","Mallow", "Cork", "Cobh"};

        public MainPage()
        {
            this.InitializeComponent();

            //Base url for getting long & lat
            string baseUrl = "https://data.smartdublin.ie/cgi-bin/rtpi/busstopinformation?stopid=";
            //End url to get json formatted api
            string endUrl = "&format=json";
            //Variable for storing station code selected
            string stationSelectedforCo;

            //Array to store all latitudes of all stations
            double[] latAllStations = new double[28];
            //Array to store all longitudes of all stations
            double[] lonAllStations = new double[28];

            //for loop runs through all stations
            for (int i = 0; i < stationCodes.Length; i++)
            {
                //Apply station[i] coded for geting station[i] long, lat & displaystopid
                stationSelectedforCo = stationCodes[i];

                //Adapted from https://stackoverflow.com/questions/36516146/parsing-json-in-uwp
                //Creates new instance of httpClient
                HttpClient client = new HttpClient();

                //gets client BaseAddress using manually built 'string', depending on station
                client.BaseAddress = new Uri(baseUrl + stationSelectedforCo + endUrl);

                //Accepts Json format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Get response via url
                HttpResponseMessage response = client.GetAsync(baseUrl + stationSelectedforCo + endUrl).Result;

                //Populate result with response
                var result = response.Content.ReadAsStringAsync().Result;

                //Populate obj with 'readable' items via result
                var obj = JsonConvert.DeserializeObject<RootLocation>(result);

                //Get latitude & longitude of station
                string latStation = obj.results[0].latitude;
                string lonStation = obj.results[0].longitude;

                //Convert/parse latitude/longitude into doubles
                double doubleLatStation = double.Parse(latStation);
                double doubleLonStation = double.Parse(lonStation);

                //Apply longitude/latitude into correct index
                latAllStations[i] = doubleLatStation;
                lonAllStations[i] = doubleLonStation;
            }

            //Creates a list of landmarks
            var MyLandmarks = new List<MapElement>();

            //Centers map on Ireland
            BasicGeoposition snPosition = new BasicGeoposition { Latitude = 53.4239, Longitude = -7.9407 };
            Geopoint snPoint = new Geopoint(snPosition);

            //Used to populate map with landmarks
            var LandmarksLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = MyLandmarks
            };

            //Adds a layer to map so Landmarks can be added
            MapControl1.Layers.Add(LandmarksLayer);

            //Sets positon/zoom of map
            MapControl1.Center = snPoint;
            MapControl1.ZoomLevel = 8;
            // Add the MapControl and the specify maps authentication key.
            MapControl1.MapServiceToken = "4eHjP7348hDlfxfexoHq~b19fzIoJ9f9tKXS6zjtgzQ~AkcMoHnOY06TTP7cZlx7HU6p51K8tdzCES0s6AfLdGfIzxYbOiXZybD_DiYZ_nd0";

            //creates new push pin
            PushPin pushPin = new PushPin();

            //Populates map with pushpins via long & lat of each station, also includes station names
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

        //This internal class is used for getting/setting pushpins for map
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

        /*
         * This method is called when a user clicks a button. It is used to call a method getLocation_Click in MyLocation.cs,
         * for getting the closest station to the users location based on longitude and latitude. This method is also used to
         * populate all the distances of all stations in relation to the user.
         */
        async void callForLocation_Click(object sender, RoutedEventArgs e)
        {
            //Creates a new instance of MediaPlayer for sounds
            MediaPlayer player = new MediaPlayer();

            //Gives a folder location to find media
            Windows.Storage.StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            //Loads which file to use for playing sound
            Windows.Storage.StorageFile file = await folder.GetFileAsync("Click.mp3");

            //So sound doesn't play automatically
            player.AutoPlay = false;
            //Selects which file to use for playing sound
            player.Source = MediaSource.CreateFromStorageFile(file);
            //Play sound
            player.Play();

            //Creates new instance of MyLocation
            MyLocation location = new MyLocation();
            //Calls getLocation_Click in MyLocation class
            location.getLocation_Click(sender, e);

            //Retrieve values from MyLocation to display in xaml
            string latitude = MyLocation.latitude;
            string longitude = MyLocation.longitude;
            string details = MyLocation.fullDetails2;
            string titleOrigin = MyLocation.originTitle;
            //Instantiate double array
            double[] d = new double[28];

            //Populate array with distance values from MyLocation class
            for (int i=0; i<stationCodes.Length;i++)
            {
                
                d[i] = MyLocation.distances[i];
            }

            //Display error to user if values are empty
            if (latitude == "Empty")
            {
                LatitudeTextBox.Text = "\tAn error occured, please wait 15 seconds and try again";

            }
            else
            {
                //Place values in TextBoxes in xaml for user
                LatitudeTextBox.Text = "\t"+latitude+"\n";
                LongitudeTextBox.Text = "\t" + longitude + "\n";
                detailsTextBox.Text = details;
                originTitleTextBox.Text = "\t"+titleOrigin+"\n";

                LatitudeTextBox2.Text = "\t" + latitude + "\n";
                LongitudeTextBox2.Text = "\t" + longitude + "\n";
                DistanceBox.Text = "";

                //Build string of station names and distances and display it to the user (xaml),
                //Make distances to 2 decimal points
                for (int i = 0; i < stationCodes.Length; i++)
                {
                    DistanceBox.Text += stationNames[i] + ": " + d[i].ToString("#.##") + "km\n";
                }
            }
        }

        //Method used for displaying details to user depending on which station is selected via the combobox.
        //Lines 252-259, 275-286 explained in callForLocation_Click.
        async void select_OnClick(object sender, RoutedEventArgs e)
        {
            MediaPlayer player = new MediaPlayer();

            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("Click.mp3");

            player.AutoPlay = false;
            player.Source = MediaSource.CreateFromStorageFile(file);
            player.Play();
            
            //Gets index & value of selected item in combobox
            selectIndex = ComboxBox1.SelectedIndex;
            object currentText = ComboxBox1.SelectedValue;

            //Make sure user selects an index (error checking)
            if (selectIndex == -1)
            {
                stationDetailsTextBox.Text = "Please select a station";
            }
            else
            {
                //Get station code of the selected index
                string stationSelected = stationCodes[selectIndex];

                string baseUrl = "https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=";
                string endUrl = "&format=json";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl + stationSelected + endUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(baseUrl + stationSelected + endUrl).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                var obj = JsonConvert.DeserializeObject<RootObject>(result);

                string fullDetails = obj.ToString();

                //if else for error checking
                if (fullDetails != "")
                {
                    //Display selected stations details to user.
                    stationDetailsTextBox.Text = fullDetails;
                }
                else
                {
                    stationDetailsTextBox.Text = "No current trains leaving from that station right now";
                }
            }
        }

        //The following methods are used to save the most recent tab / text via localSettings 
        //for when the user opens up the app again. Taken from lab 4
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            try
            {
                //remembers the most recent tab/text and stores into local storage
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
            //Loads most recent pivot
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            localSettings.Values["currentTab"] = pvtTabs.SelectedIndex;
        }

        private void preferedStation_Changed(object sender, TextChangedEventArgs e)
        {
            //Loads text
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            localSettings.Values["prefStation"] = preferedStation.Text;
        }
    }
}
