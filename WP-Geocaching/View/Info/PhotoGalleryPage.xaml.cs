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
using Microsoft.Phone.Shell;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Resources.Localization;

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
            SetPreviousButton();
            SetNextButton();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            int index = Convert.ToInt16(NavigationContext.QueryString["Index"]);
            photoGalleryPageViewModel.LoadFullsizePhoto(index);
            base.OnNavigatedTo(e);
        }

        private void SetNextButton()
        {
            ApplicationBarIconButton nextButton = new ApplicationBarIconButton();
            nextButton.IconUri = new Uri("Resources/Images/appbar.next.rest.png", UriKind.Relative);
            nextButton.Text = AppResources.NextButton;
            nextButton.Click += (sender, e) =>
            {
                photoGalleryPageViewModel.LoadNext();
            };
            ApplicationBar.Buttons.Add(nextButton);
        }

        private void SetPreviousButton()
        {
            ApplicationBarIconButton previousButton = new ApplicationBarIconButton();
            previousButton.IconUri = new Uri("Resources/Images/appbar.back.rest.png", UriKind.Relative);
            previousButton.Text = AppResources.PreviousButton;
            previousButton.Click += (sender, e) =>
            {
                photoGalleryPageViewModel.LoadPrevious();
            };
            ApplicationBar.Buttons.Add(previousButton);
        }
    }
}