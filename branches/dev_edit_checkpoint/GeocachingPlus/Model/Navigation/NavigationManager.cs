using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GeocachingPlus.Model.Navigation
{
    public class NavigationManager
    {
        public enum Params
        {
            CheckpointId = 1,
            IsAppBarEnabled = 2,
            Index = 3,
        }

        private const string OneParamPattern = "{0}?{1}={2}";

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

        public void NavigateToInfoPivot(Cache cache, bool isAppBarEnabled)
        {
            Repository.CurrentCache = cache;
            Navigate(String.Format(OneParamPattern, "//View/Info/InfoPivot.xaml",
                Params.IsAppBarEnabled, isAppBarEnabled));
        }

        public void NavigateToSearchBingMap(Cache cache)
        {
            Repository.CurrentCache = cache;
            Navigate("//View/SearchBingMap.xaml");
        }

        public void NavigateToCheckpoints()
        {
            Navigate("//View/Checkpoints/Checkpoints.xaml");
        }

        public void NavigateToCreateCheckpoint()
        {
            Navigate("//View/Checkpoints/CreateCheckpoint.xaml");
        }

        public void NavigateToEditCheckpoint(string checkpointId)
        {
            Navigate(String.Format(OneParamPattern, "//View/Checkpoints/CreateCheckpoint.xaml", Params.CheckpointId, checkpointId));
        }

        public void NavigateTologbook(Cache cache)
        {
            Repository.CurrentCache = cache;
            Navigate("//View/Info/PhotoGalleryPage.xaml");
        }

        public void NavigateToCompass(Cache cache, string checkpointId)
        {
            Repository.CurrentCache = cache;
            Navigate(String.Format(OneParamPattern, "//View/Compass/CompassPage.xaml",
                Params.CheckpointId, checkpointId));
        }

        public void NavigateToPhotoGallery(IEnumerable<Photo> photos, int index)
        {
            Repository.CurrentPhotos = photos.ToList();
            Navigate(String.Format(OneParamPattern, "//View/Info/PhotoGalleryPage.xaml",
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
