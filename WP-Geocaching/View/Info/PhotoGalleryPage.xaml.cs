using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP_Geocaching.Model;
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
            DataContext = photoGalleryPageViewModel;
            SetPreviousButton();
            SetNextButton();
            ResetImageTranslate();
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

        private const double delta = 50;
        private double limitTranslateX;
        private double limitTranslateY;

        private Point parentPosition;
        private Point childrenPosition;

        private void GestureListenerDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            if ((e.Direction != System.Windows.Controls.Orientation.Horizontal) || (imageTransform.ScaleX > 1)) return;

            var abs = Math.Abs(panelDragHorizontal);

            if (abs <= 75) return;

            if (panelDragHorizontal > 0) photoGalleryPageViewModel.LoadPrevious(ResetImageTranslate);
            else photoGalleryPageViewModel.LoadNext(ResetImageTranslate);

            e.Handled = true;
        }

        private void GestureListenerDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            //panelDragHorizontal += e.HorizontalChange;

            //var newTranslate = initialTranslateX + panelDragHorizontal * 0.7;

            //var limitTranslate = childrenPosition.X * imageTransform.ScaleX  - parentPosition.X;
            //imageTransform.TranslateX = GetCorrectedTranslate(newTranslate, limitTranslate, (image.ActualWidth - childrenPosition.X) * imageTransform.ScaleX - ContentPanel.ActualWidth + parentPosition.X);

            //panelDragVertical += e.VerticalChange;

            //newTranslate = initialTranslateY + panelDragVertical * 0.7;
            //limitTranslate = image.ActualHeight * imageTransform.ScaleY / 2 - 50;
            //delta = image.ActualHeight / 2;
            //imageTransform.TranslateY = GetCorrectedTranslate(newTranslate, limitTranslate) + delta;

            panelDragHorizontal += e.HorizontalChange;
            imageTransform.TranslateX = initialTranslateX + panelDragHorizontal * 0.7;
            panelDragVertical += e.VerticalChange;
            imageTransform.TranslateY = initialTranslateY + panelDragVertical * 0.7;
        }


        private void GestureListenerDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            panelDragHorizontal = 0;
            panelDragVertical = 0;
            initialTranslateX = imageTransform.TranslateX;
            initialTranslateY = imageTransform.TranslateY;
            parentPosition = e.GetPosition(ContentPanel);
            childrenPosition = e.GetPosition(image);

            LogManager.Log(parentPosition.ToString());
            LogManager.Log(childrenPosition.ToString());
        }


        private double GetCorrectedTranslate(double translate, double point, double size)
        {
            if (point + delta < translate)
            {
                return point + delta;
            }
            if (- size - delta > translate)
            {
                return - size - delta;
            }
            return translate;
        }

        private double initialScale;

        private void GestureListenerPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            initialScale = imageTransform.ScaleX;
            imageTransform.CenterX = (e.GetPosition(image, 0).X + e.GetPosition(image, 1).X) / 2;
            imageTransform.CenterY = (e.GetPosition(image, 0).Y + e.GetPosition(image, 1).Y) / 2;
        }

        private void GestureListenerPinchDelta(object sender, PinchGestureEventArgs e)
        {          
            imageTransform.ScaleX = imageTransform.ScaleY = e.DistanceRatio * initialScale > 1 ? initialScale * e.DistanceRatio : 1;
        }
        
        private void ResetImageTranslate()
        {
            imageTransform.CenterX = image.ActualWidth / 2;
            imageTransform.CenterY = image.ActualHeight / 2;
            imageTransform.ScaleX = 1;
            imageTransform.ScaleY = 1;
            imageTransform.TranslateX = 0;
            imageTransform.TranslateY = 0;
        }
    }
}