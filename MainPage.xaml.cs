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

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //Adapted from https://stackoverflow.com/questions/36516146/parsing-json-in-uwp

            HttpClient client = new HttpClient();
            client.BaseAddress =
                new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=GALWY&format=json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response =
                client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=GALWY&format=json").Result;
            var result = response.Content.ReadAsStringAsync().Result;

            var obj = JsonConvert.DeserializeObject<RootObject>(result);

            //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXX:" + obj.stopid);
            //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXX:{0}", ToString());
            Debug.WriteLine(obj);
            //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXX:" + obj.stopid);
            //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXX:" + obj.stopid);

        }


        

 

    }
}
