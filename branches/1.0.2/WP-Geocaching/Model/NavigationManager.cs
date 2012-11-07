using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace WP_Geocaching.Model
{
    public class NavigationManager : INavigationManager
    {
        public enum Params
        {
            Id = 0,
            CheckpointId = 1,
            IsAppBarEnabled = 2,
            Index = 3
        }

        private static string OneParamsPattern = "{0}?{1}={2}";
        private static string TwoParamsPattern = "{0}?{1}={2}&{3}={4}";

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

        public void NavigateToInfoPivot(string currentId, bool isAppBarEnabled)
        {
            Navigate(String.Format(TwoParamsPattern, "//View/Info/InfoPivot.xaml",
                Params.Id, currentId,
                Params.IsAppBarEnabled, isAppBarEnabled));
        }

        public void NavigateToSearchBingMap(string currentId)
        {
            Navigate(String.Format(OneParamsPattern, "//View/SearchBingMap.xaml",
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

        public void NavigateToNotebook(string currentId)
        {
            Navigate(String.Format(OneParamsPattern, "//View/Info/PhotoGalleryPage.xaml",
                Params.Id, currentId));
        }

        public void NavigateToCompass(string currentId, string checkpointId)
        {
            Navigate(String.Format(TwoParamsPattern, "//View/Compass/CompassPage.xaml",
                Params.Id, currentId, 
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
