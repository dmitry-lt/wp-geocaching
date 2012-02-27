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

namespace WP_Geocaching
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void ChooseCache_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/BingMap.xaml", UriKind.Relative));
        }

        private void ChooseCompass_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Compass/CompassPage.xaml", UriKind.Relative));
        }

        private void Favorites_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Favorites.xaml", UriKind.Relative));
        }
    }
}