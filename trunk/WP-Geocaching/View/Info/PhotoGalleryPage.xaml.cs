using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Resources.Localization;
using WP_Geocaching.Model;

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
            nextButton.Click += (sender, e) => photoGalleryPageViewModel.LoadNext(ResetImageTranslate);
            ApplicationBar.Buttons.Add(nextButton);
        }

        private void SetPreviousButton()
        {
            var previousButton = new ApplicationBarIconButton
                                     {
                                         IconUri = new Uri("Resources/Images/appbar.back.rest.png", UriKind.Relative),
                                         Text = AppResources.PreviousButton
                                     };
            previousButton.Click += (sender, e) => photoGalleryPageViewModel.LoadPrevious(ResetImageTranslate);
            ApplicationBar.Buttons.Add(previousButton);
        }

        private double panelDragHorizontal;
        private double panelDragVertical;
        private double initialTranslateX;
        private double initialTranslateY;

        private void GestureListenerDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            if ((e.Direction != System.Windows.Controls.Orientation.Horizontal) || (imageTranslate.ScaleX > 1)) return;

            var abs = Math.Abs(panelDragHorizontal);

            if (abs <= 75) return;

            if (panelDragHorizontal > 0) photoGalleryPageViewModel.LoadPrevious(ResetImageTranslate);
            else photoGalleryPageViewModel.LoadNext(ResetImageTranslate);

            e.Handled = true;
        }

        private void GestureListenerDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            //panelDragHorizontal += e.HorizontalChange;
            //double x = initialTranslateX + panelDragHorizontal * 0.7;
            //if ((image.ActualHeight / 2 > x) && (panelDragHorizontal < 0)) imageTranslate.TranslateX = image.ActualHeight / 2;
            //else if ((-image.ActualHeight / 2 < x) && (panelDragHorizontal > 0)) imageTranslate.TranslateX = image.ActualHeight / 2;
            //else imageTranslate.TranslateX = x;

            panelDragHorizontal += e.HorizontalChange;
            imageTranslate.TranslateX = initialTranslateX + panelDragHorizontal * 0.7;
            panelDragVertical += e.VerticalChange;
            imageTranslate.TranslateY = initialTranslateY + panelDragVertical * 0.7;
        }

        private void GestureListenerDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            panelDragHorizontal = 0;
            panelDragVertical = 0;
            initialTranslateX = imageTranslate.TranslateX;
            initialTranslateY = imageTranslate.TranslateY;
        }

        private double initialScale;

        private void GestureListenerPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            initialScale = imageTranslate.ScaleX;
        }

        private void GestureListenerPinchDelta(object sender, PinchGestureEventArgs e)
        {
            imageTranslate.ScaleX = imageTranslate.ScaleY = e.DistanceRatio > 1 ? initialScale * e.DistanceRatio : 1;
        }

        private void ResetImageTranslate()
        {
            imageTranslate.ScaleX = 1;
            imageTranslate.ScaleY = 1;
            imageTranslate.TranslateX = 0;
            imageTranslate.TranslateY = 0;
        }
    }
}