using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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

        public void Navigate(string uri)
        {
            this.Navigate(uri, null);
        }
        public void Navigate(string uri, string currentId)
        {
            PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
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
        public void NavigateToDetails(string CurrentId)
        {
            this.Navigate("//View/Details.xaml", CurrentId);
        }
        public void NavigateToSearchBingMap(string CurrentId)
        {
            this.Navigate("//View/SearchBingMap.xaml", CurrentId);
        }
        public void NavigateToCheckpoints()
        {
            this.Navigate("//View/Checkpoints/Checkpoints.xaml");
        }
        public void NavigateToCreateCheckpoint()
        {
            this.Navigate("//View/Checkpoints/CreateCheckpoint.xaml");
        }
    }
}
