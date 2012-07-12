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
using WP_Geocaching.Model;

namespace WP_Geocaching.View.Info
{
    public partial class ThreePhotoItem : UserControl
    {
        public ThreePhotoItem()
        {
            InitializeComponent();
        }

        private void Image_GotFocus(object sender, RoutedEventArgs e)
        {
            var index = (int)(((Image)sender).Tag);
            NavigationManager.Instance.NavigateToPhotoGallery(index);
        }
    }
}
