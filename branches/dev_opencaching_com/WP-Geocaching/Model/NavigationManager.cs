using System;
using System.Windows;
using Microsoft.Phone.Controls;
using WP_Geocaching.Model.Api;

namespace WP_Geocaching.Model
{
    public class NavigationManager
    {
        public enum Params
        {
            Id = 0,
            CheckpointId = 1,
            IsAppBarEnabled = 2,
            Index = 3,
            CacheProvider = 4,
        }

        private static string OneParamsPattern = "{0}?{1}={2}";
        private static string TwoParamsPattern = "{0}?{1}={2}&{3}={4}";
        private static string ThreeParamsPattern = "{0}?{1}={2}&{3}={4}&{5}={6}";

        private static NavigationManager instance;

        public static NavigationManager Instance
        {
            get
            {
                return instance ?? (instance = new NavigationManager());
            }
        }

        private NavigationManager() { }

        private void Navigate(string uri)
        {
            var frame = Application.Current.RootVisual as PhoneApplicationFrame;

            if (frame == null)
            {
                return;
            }

            frame.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        public void NavigateToInfoPivot(string currentId, CacheProvider cacheProvider, bool isAppBarEnabled)
        {
            Navigate(String.Format(ThreeParamsPattern, "//View/Info/InfoPivot.xaml",
                Params.Id, currentId,
                Params.CacheProvider, cacheProvider,
                Params.IsAppBarEnabled, isAppBarEnabled));
        }

        public void NavigateToSearchBingMap(string currentId, CacheProvider cacheProvider)
        {
            Navigate(String.Format(TwoParamsPattern, "//View/SearchBingMap.xaml",
                Params.CacheProvider, cacheProvider,
                Params.Id, currentId));
        }

        public void NavigateToCheckpoints()
        {
            Navigate("//View/Checkpoints/Checkpoints.xaml");
        }

        public void NavigateToCreateCheckpoint()
        {
            Navigate("//View/Checkpoints/CreateCheckpoint.xaml");
        }

        public void NavigateToNotebook(string currentId, CacheProvider cacheProvider)
        {
            Navigate(String.Format(TwoParamsPattern, "//View/Info/PhotoGalleryPage.xaml",
                Params.CacheProvider, cacheProvider,
                Params.Id, currentId));
        }

        public void NavigateToCompass(string currentId, CacheProvider cacheProvider, string checkpointId)
        {
            Navigate(String.Format(ThreeParamsPattern, "//View/Compass/CompassPage.xaml",
                Params.Id, currentId,
                Params.CacheProvider, cacheProvider,
                Params.CheckpointId, checkpointId));
        }

        public void NavigateToPhotoGallery(int index)
        {
            Navigate(String.Format(OneParamsPattern, "//View/Info/PhotoGalleryPage.xaml",
                Params.Index, index));
        }

        public void  NavigateToSettings()
        {
            Navigate("//View/Settings/Settings.xaml");
        }

        public void NavigateToFavorites()
        {
            Navigate("/View/Favorites/FavoritesPage.xaml");
        }

        public void NavigateToBingMap()
        {
            Navigate("/View/BingMap.xaml");
        }
    }
}
