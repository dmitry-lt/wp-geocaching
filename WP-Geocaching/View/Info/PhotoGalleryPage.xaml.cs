using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
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

        private double contentPanelHalfWidth;
        private double contentPanelHalfHight;
        private double panelDragHorizontal;
        private double panelDragVertical;
        private double initialTranslateX;
        private double initialTranslateY;

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
                panelDragHorizontal += e.HorizontalChange;
                var limitTranslate = (image.ActualWidth) * imageTransform.ScaleX / 2 - contentPanelHalfWidth;
                var newTranslate = initialTranslateX + panelDragHorizontal;
                if (IsNormalTranslate(newTranslate, limitTranslate, e.HorizontalChange))
                {
                    imageTransform.TranslateX = newTranslate;
                }
                else
                {
                    panelDragHorizontal -= e.HorizontalChange;
                }

                panelDragVertical += e.VerticalChange;
                limitTranslate = (image.ActualHeight) * imageTransform.ScaleX / 2 - contentPanelHalfHight;
                newTranslate = initialTranslateY + panelDragVertical;
                if (IsNormalTranslate(newTranslate, limitTranslate, e.VerticalChange))
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
            }
        }


        private void GestureListenerDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            panelDragHorizontal = 0;
            panelDragVertical = 0;

            initialTranslateX = imageTransform.TranslateX;
            initialTranslateY = imageTransform.TranslateY;

            contentPanelHalfWidth = ContentPanel.ActualWidth / 2;
            contentPanelHalfHight = ContentPanel.ActualHeight / 2;
        }


        private bool IsNormalTranslate(double translate, double limitTranslate, double difference)
        {
            if (limitTranslate < translate && difference >= 0)
            {
                return false;
            }
            if (-limitTranslate > translate && difference <= 0)
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
                    return Math.Max(1, ((BitmapSource)image.Source).PixelWidth / ContentPanel.ActualWidth) * Math.Sqrt(2);
                case PageOrientation.LandscapeRight:
                case PageOrientation.LandscapeLeft:
                case PageOrientation.Landscape:
                    return Math.Max(1, ((BitmapSource)image.Source).PixelHeight / ContentPanel.ActualHeight) * Math.Sqrt(2);
            }
            return 1;
        }

        private void SetImageTranslate(double previousScale)
        {
            var difference = imageTransform.ScaleX - previousScale;

            imageTransform.TranslateX += (imageHalfWidth - midPoint.X) * difference;
            imageTransform.TranslateY += (imageHalfHight - midPoint.Y) * difference;
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
            SetContentPanelCLlip();
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
            SetContentPanelCLlip();
        }

        private void SetContentPanelCLlip()
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