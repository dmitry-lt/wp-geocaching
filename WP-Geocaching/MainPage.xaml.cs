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
using WP_Geocaching.Model;

namespace WP_Geocaching
{
    public partial class MainPage : PhoneApplicationPage
    {
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
            NavigationService.Navigate(new Uri("/View/Favorites/FavoritesPage.xaml", UriKind.Relative));
        }

        private void SearchCache_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            if (settings.LastSoughtCacheId > 0)
            {
                NavigationManager.Instance.NavigateToSearchBingMap(settings.LastSoughtCacheId.ToString());
            }
            else
            {
                this.NoSouhgtCacheMessage.Visibility = System.Windows.Visibility.Visible;
                System.Threading.Timer timer = new System.Threading.Timer(DisposeTimerAndCollapseMessage);
                timer.Change(3000, 0);
            }
        }

        private void DisposeTimerAndCollapseMessage(object state)
        {
            System.Threading.Timer t = (System.Threading.Timer)state;
            t.Dispose();
            this.Dispatcher.BeginInvoke(() =>
            {
                this.NoSouhgtCacheMessage.Visibility = System.Windows.Visibility.Collapsed;
            });        
        }
    }
}