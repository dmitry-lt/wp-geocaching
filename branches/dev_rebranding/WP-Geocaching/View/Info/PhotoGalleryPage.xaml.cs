using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Navigation;
using GeocachingPlus.ViewModel;
using GeocachingPlus.Resources.Localization;

namespace GeocachingPlus.View.Info
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
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            int index = Convert.ToInt16(NavigationContext.QueryString[NavigationManager.Params.Index.ToString()]);
            photoGalleryPageViewModel.Photos = Repository.CurrentPhotos;
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

        private double contentPanelHalfWidth;
        private double contentPanelHalfHight;
        private double panelDragHorizontal;

        private void GestureListenerDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            if ((e.Direction != System.Windows.Controls.Orientation.Horizontal) || (imageTransform.ScaleX > 1)) return;

            if (panelDragHorizontal > 15) photoGalleryPageViewModel.LoadPrevious(ResetImageTranslate);
            else if (panelDragHorizontal < -15) photoGalleryPageViewModel.LoadNext(ResetImageTranslate);

            e.Handled = true;
        }

        private void GestureListenerDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            if (imageTransform.ScaleX > 1)
            {
                var limitTranslate = (image.ActualWidth) * imageTransform.ScaleX / 2 - contentPanelHalfWidth;
                imageTransform.TranslateX = SetNormalizeTranslate(imageTransform.TranslateX, limitTranslate,
                                                                  e.HorizontalChange);

                limitTranslate = (image.ActualHeight) * imageTransform.ScaleX / 2 - contentPanelHalfHight;
                imageTransform.TranslateY = SetNormalizeTranslate(imageTransform.TranslateY, limitTranslate,
                                                                  e.VerticalChange);
            }
            else
            {
                panelDragHorizontal += e.HorizontalChange;
            }
        }


        private void GestureListenerDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            panelDragHorizontal = 0;

            contentPanelHalfWidth = ContentPanel.ActualWidth / 2;
            contentPanelHalfHight = ContentPanel.ActualHeight / 2;
        }

        private double SetNormalizeTranslate(double previousTranslate, double limitTranslate, double change)
        {
            var newTranslate = previousTranslate + change;
            if (IsNormalTranslate(newTranslate, limitTranslate))
            {
                return newTranslate;
            }
            else
            {
                return previousTranslate * limitTranslate > 0 ? limitTranslate : -limitTranslate;
            }
        }

        private bool IsNormalTranslate(double translate, double limitTranslate)
        {

            if (Math.Abs(limitTranslate) < translate)
            {
                return false;
            }
            if (-Math.Abs(limitTranslate) > translate)
            {
                return false;
            }
            return true;
        }


        private double imageHalfWidth;
        private double imageHalfHight;
        private double initialScale;
        private double maxScale;
        private Point midPoint;

        private void GestureListenerPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            initialScale = imageTransform.ScaleX;

            maxScale = GetMaxScale();

            imageHalfWidth = image.ActualWidth / 2;
            imageHalfHight = image.ActualHeight / 2;

            var firstPositionPoint = e.GetPosition(image, 0);
            var secondPositionPoint = e.GetPosition(image, 1);

            midPoint = new Point((firstPositionPoint.X + secondPositionPoint.X) / 2,
                                     (firstPositionPoint.Y + secondPositionPoint.Y) / 2);
        }

        private void GestureListenerPinchDelta(object sender, PinchGestureEventArgs e)
        {
            if (!(IsPointOnImage(e.GetPosition(image, 0)) && IsPointOnImage(e.GetPosition(image, 1))))
            {
                return;
            }

            var previousScale = imageTransform.ScaleX;

            imageTransform.ScaleX = imageTransform.ScaleY = GetNormalizeScale(initialScale * e.DistanceRatio);

            SetImageTranslate(previousScale);
        }
        
        private double GetMaxScale()
        {
            switch (Orientation)
            {
                case PageOrientation.PortraitUp:
                case PageOrientation.PortraitDown:
                case PageOrientation.Portrait:
                    return Math.Max(1, ((BitmapSource)image.Source).PixelWidth / ContentPanel.ActualWidth * Math.Sqrt(2));
                case PageOrientation.LandscapeRight:
                case PageOrientation.LandscapeLeft:
                case PageOrientation.Landscape:
                    return Math.Max(1, ((BitmapSource)image.Source).PixelHeight / ContentPanel.ActualHeight * Math.Sqrt(2));
            }
            return 1;
        }

        private void SetImageTranslate(double previousScale)
        {
            var difference = imageTransform.ScaleX - previousScale;

            var limitTranslate = (image.ActualWidth) * imageTransform.ScaleX / 2 - imageHalfWidth;
            var change = (imageHalfWidth - midPoint.X) * difference;
            imageTransform.TranslateX = SetNormalizeTranslate(imageTransform.TranslateX, limitTranslate, change);

            limitTranslate = (image.ActualHeight) * imageTransform.ScaleX / 2 - imageHalfHight;
            change = (imageHalfHight - midPoint.Y) * difference;
            imageTransform.TranslateY = SetNormalizeTranslate(imageTransform.TranslateY, limitTranslate, change);
        }

        private double GetNormalizeScale(double scale)
        {
            if (scale > maxScale)
            {
                return maxScale;
            }
            else if (scale > 1)
            {
                return scale;
            }
            else
            {
                return 1;
            }
        }

        private bool IsPointOnImage(Point point)
        {
            return point.Y <= image.ActualHeight && point.Y >= 0 &&
                   point.X <= image.ActualWidth && point.X >= 0;
        }

        private void ResetImageTranslate()
        {
            imageTransform.ScaleX = 1;
            imageTransform.ScaleY = 1;

            imageTransform.TranslateX = 0;
            imageTransform.TranslateY = 0;
        }

        private void ContentPanelLoaded(object sender, RoutedEventArgs e)
        {
            SetContentPanelClip();
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
            SetContentPanelClip();
        }

        private void SetContentPanelClip()
        {
            if (ContentPanel == null)
            {
                return;
            }

            ContentPanel.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, ContentPanel.ActualWidth, ContentPanel.ActualHeight)
            };
        }
    }
}