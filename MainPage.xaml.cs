using Newtonsoft.Json;
using System;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //Adapted from https://stackoverflow.com/questions/36516146/parsing-json-in-uwp

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=GALWY&format=json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=GALWY&format=json").Result;
            var result = response.Content.ReadAsStringAsync().Result;

            var obj = JsonConvert.DeserializeObject<RootObject>(result);

            //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXX:" + obj.stopid);
            //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXX:{0}", ToString());
            Debug.WriteLine(obj);
            //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXX:" + obj.stopid);
            //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXX:" + obj.stopid);

        }


        //Created using http://json2csharp.com/
        public class Result
        {
            public string arrivaldatetime { get; set; }
            public string duetime { get; set; }
            public string departuredatetime { get; set; }
            public string departureduetime { get; set; }
            public string scheduledarrivaldatetime { get; set; }
            public string scheduleddeparturedatetime { get; set; }
            public string destination { get; set; }
            public string destinationlocalized { get; set; }
            public string origin { get; set; }
            public string originlocalized { get; set; }
            public string direction { get; set; }
            public string @operator { get; set; }
            public string additionalinformation { get; set; }
            public string lowfloorstatus { get; set; }
            public string route { get; set; }
            public string sourcetimestamp { get; set; }
            public string monitored { get; set; }


            public override string ToString()
            {
                return string.Format("From: {0}\tTo: {1}\nDeparture Date/Time: {2}\tArrival Date/Time: {3}\n", origin, destination, departuredatetime, scheduledarrivaldatetime);
            }


        }

        //Created using http://json2csharp.com/
        public class RootObject
        {
            public string errorcode { get; set; }
            public string errormessage { get; set; }
            public int numberofresults { get; set; }
            public string stopid { get; set; }
            public string timestamp { get; set; }
            public List<Result> results { get; set; }


            public override string ToString()
            {
                // Returns vars from Results class
                return string.Format("{0}", string.Join("", results));
            }

        }

    }
}
