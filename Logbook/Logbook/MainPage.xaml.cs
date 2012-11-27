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


namespace Logbook
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void showText(String text)
        {
            webBrowser1.NavigateToString(text);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GetLogbook test = new GetLogbook();
            Action<string> processLogbook;
            processLogbook = delegate(string s) { showText(s); };
            test.FetchCacheDetails(processLogbook, "GC1NKDN");
        }
    }
}