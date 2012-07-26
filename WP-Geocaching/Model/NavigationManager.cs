using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace WP_Geocaching.Model
{
    public class NavigationManager : INavigationManager
    {
        private static NavigationManager instance;

        public static NavigationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NavigationManager();
                }
                return instance;
            }
        }

        private NavigationManager() { }

        private void Navigate(string uri)
        {
            Navigate(uri, null);
        }

        private void Navigate(string uri, string currentId)
        {
            var frame = Application.Current.RootVisual as PhoneApplicationFrame;

            if (frame == null)
            {
                return;
            }
            if (currentId == null)
            {
                frame.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
            }
            else
            {
                frame.Navigate(new Uri(uri + "?ID=" + currentId, UriKind.RelativeOrAbsolute));
            }
        }

        private void Navigate(string uri, string currentId, string checkpointId)
        {
            var frame = Application.Current.RootVisual as PhoneApplicationFrame;
            frame.Navigate(new Uri(uri + "?CurrentId=" + currentId + "&CheckpointId=" + checkpointId, UriKind.RelativeOrAbsolute));
        }

        public void NavigateToInfoPivot(string CurrentId)
        {
            Navigate("//View/Info/InfoPivot.xaml", CurrentId);
        }

        public void NavigateToSearchBingMap(string CurrentId)
        {
            Navigate("//View/SearchBingMap.xaml", CurrentId);
        }

        public void NavigateToCheckpoints()
        {
            Navigate("//View/Checkpoints/Checkpoints.xaml");
        }

        public void NavigateToCreateCheckpoint()
        {
            Navigate("//View/Checkpoints/CreateCheckpoint.xaml");
        }

        public void NavigateToNotebook(string CurrentId)
        {
            Navigate("//View/Notebook.xaml", CurrentId);
        }

        public void NavigateToCompass(string currentId, string checkpointId)
        {
            Navigate("//View/Compass/CompassPage.xaml", currentId, checkpointId);
        }

        public void NavigateToPhotoGallery(int index)
        {
            var frame = Application.Current.RootVisual as PhoneApplicationFrame;
            frame.Navigate(new Uri("//View/Info/PhotoGalleryPage.xaml" + "?Index=" + index, UriKind.RelativeOrAbsolute));
        }

        public void  NavigateToSettings()
        {
            Navigate("//View/Settings/Settings.xaml");
        }
    }
}
