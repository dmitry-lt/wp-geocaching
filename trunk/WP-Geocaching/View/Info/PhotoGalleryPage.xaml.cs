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
using WP_Geocaching.ViewModel;

namespace WP_Geocaching.View.Info
{
    public partial class PhotoGalleryPage : PhoneApplicationPage
    {
        PhotoGalleryPageViewModel photoGalleryPageViewModel;

        public PhotoGalleryPage()
        {
            InitializeComponent();
            photoGalleryPageViewModel = new PhotoGalleryPageViewModel();
            this.DataContext = photoGalleryPageViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string photoUrl = NavigationContext.QueryString["ID"];
            photoGalleryPageViewModel.LoadFullsizePhoto(photoUrl);
            base.OnNavigatedTo(e);
        }
    }
}