using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

namespace JsonParserPerformance
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private double DiffMillis(DateTime from, DateTime to)
        {
            var ts = new TimeSpan(to.Ticks - from.Ticks);
            return ts.TotalMilliseconds;
        }

        private const int Count = 50;

        private double Measure(Action action)
        {
            var from = DateTime.Now;
            for (var i = 0; i <= Count; i++)
            {
                var parsedData = (GeocachingComApiCaches)JsonConvert.DeserializeObject(Data.Json, typeof(GeocachingComApiCaches));
            }
            return DiffMillis(from, DateTime.Now);
        }

        private void NewtonsoftJson()
        {
            var parsedData = (GeocachingComApiCaches)JsonConvert.DeserializeObject(Data.Json, typeof(GeocachingComApiCaches));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            textBlock1.Text = "" + Measure(NewtonsoftJson);
        }
    }
}