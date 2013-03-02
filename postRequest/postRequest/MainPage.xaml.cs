using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;

namespace postRequest
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void showText(string str)
        {
            if (this.webBrowser1.CheckAccess())

                this.webBrowser1.NavigateToString(str);

            else
            {

                this.webBrowser1.Dispatcher.BeginInvoke(() =>
                {

                    this.webBrowser1.NavigateToString(str);

                });
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Request request = new Request();
            Action<string> onResponseGot;
            onResponseGot = delegate(string s) { showText(s); };
            Dictionary<string, string> parametrs = new Dictionary<string, string>();
            parametrs.Add("mode", "searchresult");
            parametrs.Add("resultid", "195206");
            parametrs.Add("lat1", "51.736065181251796");
            parametrs.Add("lat2", "51.79980455725786");
            parametrs.Add("lon1", "10.227241516113281");
            parametrs.Add("lon2", "10.359420776367188");
            request.Post("http://www.opencaching.de/map2.php", parametrs, onResponseGot);
        }
        

    }
}