using System;
using System.Threading;
using System.Windows;
using Microsoft.Phone.Controls;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ChooseCacheClick(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.NavigateToBingMap();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.NavigateToSettings();
        }

        private void FavoritesClick(object sender, RoutedEventArgs e)
        {
            var db = new CacheDataBase();
            var dbCacheList = db.GetCacheList();

            if (dbCacheList.Count > 0)
            {
                NavigationManager.Instance.NavigateToFavorites();
            }
            else
            {
                NoFavoriteCachesMessage.Visibility = Visibility.Visible;
                var timer = new Timer(DisposeTimerAndCollapseMessage);
                timer.Change(3000, 0);
            }
        }

        private void SearchCacheClick(object sender, RoutedEventArgs e)
        {
            var settings = new Settings();
            if (settings.LastSoughtCacheId > 0)
            {
                NavigationManager.Instance.NavigateToSearchBingMap(settings.LastSoughtCacheId.ToString());
            }
            else
            {
                NoSouhgtCacheMessage.Visibility = Visibility.Visible;
                var timer = new Timer(DisposeTimerAndCollapseMessage);
                timer.Change(3000, 0);
            }
        }

        private void DisposeTimerAndCollapseMessage(object state)
        {
            var t = (Timer)state;
            t.Dispose();
            Dispatcher.BeginInvoke(() =>
            {
                NoSouhgtCacheMessage.Visibility = Visibility.Collapsed;
                NoFavoriteCachesMessage.Visibility = Visibility.Collapsed;
            });
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            NoSouhgtCacheMessage.Visibility = Visibility.Collapsed;
            NoFavoriteCachesMessage.Visibility = Visibility.Collapsed;
        }
    }
}