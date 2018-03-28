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

        


        public MainPage()
        {
            this.InitializeComponent();



            
            //Populates ComboBox
            for (int i = 0; i<stationNames.Length;i++) {
                sNames.Add(new DisplayItems()
                {
                    ID = i,
                    stationName = stationNames[i]
                });
            }

        }//MainPage


        void select_OnClick(object sender, RoutedEventArgs e)
        {

            selectIndex = ComboxBox1.SelectedIndex;
            object currentText = ComboxBox1.SelectedValue;
            Debug.WriteLine(currentText);

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

            Debug.WriteLine("fullDetails: "+fullDetails);

            if (fullDetails != "") {

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
