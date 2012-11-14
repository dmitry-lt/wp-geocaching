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

namespace Description
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void showText(String text)
        {
            InfoBrowser.NavigateToString(text);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GetDescription test = new GetDescription();
            Action<string> processDescription;
            processDescription = delegate(string s) { showText(s); };
            test.FetchCacheDetails(processDescription, "GC1NKDN");
        }


    }
}