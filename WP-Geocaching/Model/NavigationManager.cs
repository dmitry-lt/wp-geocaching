﻿using System;
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

        private void Navigate(string uri)
        {
            this.Navigate(uri, null);
        }

        private void Navigate(string uri, string currentId)
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

        private void Navigate(string uri, string currentId, string checkpointId)
        {
            PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
            frame.Navigate(new Uri(uri + "?CurrentId=" + currentId + "&CheckpointId=" + checkpointId, UriKind.RelativeOrAbsolute));
        }

        public void NavigateToInfoPivot(string CurrentId)
        {
            this.Navigate("//View/Info/InfoPivot.xaml", CurrentId);
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
            this.Navigate("//View/Checkpoints/CreateCheckpointPivot.xaml");
        }

        public void NavigateToNotebook(string CurrentId)
        {
            this.Navigate("//View/Notebook.xaml", CurrentId);
        }
        public void NavigateByCompass(string currentId, string checkpointId)
        {
            this.Navigate("//View/Compass/CompassPage.xaml", currentId, checkpointId);
        }
    }
}
