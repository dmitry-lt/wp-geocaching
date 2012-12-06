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

        private double Measure(Action action)
        {
            var count = Convert.ToInt32(textBox1.Text);

            var from = DateTime.Now;
            for (var i = 0; i <= count; i++)
            {
                action();
            }
            return DiffMillis(from, DateTime.Now);
        }

        private void NewtonsoftJson()
        {
            var parsedData = (GeocachingComApiCaches)JsonConvert.DeserializeObject(Data.Json, typeof(GeocachingComApiCaches));
        }

        private void JsonToolkit()
        {
            var parsedData = ComputerBeacon.Json.Parser.Parse(Data.Json);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            textBlock1.Text = "" + Measure(NewtonsoftJson);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            textBlock2.Text = "" + Measure(JsonToolkit);
        }
    }
}