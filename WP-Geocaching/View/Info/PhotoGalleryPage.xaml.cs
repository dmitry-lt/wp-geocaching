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
        private double deltaX;
        private double deltaY;

        public PhotoGalleryPage()
        {
            InitializeComponent();
            photoGalleryPageViewModel = new PhotoGalleryPageViewModel();
            DataContext = photoGalleryPageViewModel;
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

        

        private Point parentPosition;
        private Point childrenPosition;

        private void GestureListenerDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            if ((e.Direction != System.Windows.Controls.Orientation.Horizontal) || (imageTransform.ScaleX > 1)) return;

            if (panelDragHorizontal > 0) photoGalleryPageViewModel.LoadPrevious(ResetImageTranslate);
            else photoGalleryPageViewModel.LoadNext(ResetImageTranslate);

            e.Handled = true;
        }

        private void GestureListenerDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            if (imageTransform.ScaleX > 1)
            {
                double limitTranslate;
                double newTranslate;

                panelDragHorizontal += e.HorizontalChange;
                limitTranslate = (image.ActualWidth) * imageTransform.ScaleX / 2 - deltaX;
                newTranslate = initialTranslateX + panelDragHorizontal;
                if (IsCorrectedTranslate(newTranslate, limitTranslate, e.HorizontalChange))
                {
                    imageTransform.TranslateX = newTranslate;
                }
                else
                {
                    panelDragHorizontal -= e.HorizontalChange;
                }

                panelDragVertical += e.VerticalChange;
                limitTranslate = (image.ActualHeight) * imageTransform.ScaleX / 2 - deltaY;
                newTranslate = initialTranslateY + panelDragVertical;
                if (IsCorrectedTranslate(newTranslate, limitTranslate, e.VerticalChange))
                {
                    imageTransform.TranslateY = newTranslate;
                }
                else
                {
                    panelDragVertical -= e.VerticalChange;
                }
            }
            else
            {
                panelDragHorizontal += e.HorizontalChange;
                imageTransform.TranslateX = initialTranslateX + panelDragHorizontal;
            }
        }


        private void GestureListenerDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            deltaX = ContentPanel.ActualWidth / 2 - 50;
            deltaY = ContentPanel.ActualHeight / 2 - 50;
            panelDragHorizontal = 0;
            panelDragVertical = 0;
            initialTranslateX = imageTransform.TranslateX;
            initialTranslateY = imageTransform.TranslateY;
        }


        private bool IsCorrectedTranslate(double translate, double limitTranslate, double changing)
        {
            if (limitTranslate < translate && changing > 0)
            {
                return false;
            }
            else if (-limitTranslate > translate && changing < 0)
            {
                return false;
            }
            return true;
        }

        private double initialScale;

        private void GestureListenerPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            initialScale = imageTransform.ScaleX;
        }

        private void GestureListenerPinchDelta(object sender, PinchGestureEventArgs e)
        {
            imageTransform.ScaleX = imageTransform.ScaleY = e.DistanceRatio * initialScale > 1 ? initialScale * e.DistanceRatio : 1;
        }

        private void ResetImageTranslate()
        {
            imageTransform.ScaleX = 1;
            imageTransform.ScaleY = 1;
            imageTransform.TranslateX = 0;
            imageTransform.TranslateY = 0;
        }
    }
}