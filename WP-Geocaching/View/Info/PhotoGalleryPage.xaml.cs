using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.View.Info
{
    public partial class PhotoGalleryPage : PhoneApplicationPage
    {
        private PhotoGalleryPageViewModel photoGalleryPageViewModel;

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
            var nextButton = new ApplicationBarIconButton
                                 {
                                     IconUri = new Uri("Resources/Images/appbar.next.rest.png", UriKind.Relative),
                                     Text = AppResources.NextButton
                                 };
            nextButton.Click += (sender, e) => photoGalleryPageViewModel.LoadNext();
            ApplicationBar.Buttons.Add(nextButton);
        }

        private void SetPreviousButton()
        {
            var previousButton = new ApplicationBarIconButton
                                     {
                                         IconUri = new Uri("Resources/Images/appbar.back.rest.png", UriKind.Relative),
                                         Text = AppResources.PreviousButton
                                     };
            previousButton.Click += (sender, e) => photoGalleryPageViewModel.LoadPrevious();
            ApplicationBar.Buttons.Add(previousButton);
        }

        private void GestureListenerDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            {
                var abs = Math.Abs(panelDragHorizontal);
                if (abs > 75)
                {
                    if (panelDragHorizontal > 0) photoGalleryPageViewModel.LoadPrevious();
                    else photoGalleryPageViewModel.LoadNext();

                    e.Handled = true;
                }
            }
            imageTranslate.TranslateX = 0;
        }


        double panelDragHorizontal;
        private void GestureListenerDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            {
                panelDragHorizontal += e.HorizontalChange;

                imageTranslate.TranslateX = panelDragHorizontal * 0.7;
            }

        }

        private void GestureListenerDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            panelDragHorizontal = 0;
        }
    }
}