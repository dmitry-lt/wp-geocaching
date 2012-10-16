using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WP_Geocaching.Model;
using WP_Geocaching.Model.Api;

namespace WP_Geocaching.ViewModel
{
    public class PhotoGalleryPageViewModel : BaseViewModel
    {
        private double maxHeight;
        private ImageSource imageSource;
        private int currentIndex;

        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                imageSource = value;
                NotifyPropertyChanged("ImageSource");
            }
        }

        public double MaxHeight
        {
            get
            {
                return maxHeight;
            }
            set
            {
                maxHeight = value;
                NotifyPropertyChanged("MaxHeight");
            }
        }

        public void LoadFullsizePhoto(int index)
        {
            currentIndex = index;
            ApiManager.Instance.ProcessPhoto(SetImageSource, index);
        }

        public void SetImageSource(Photo source, int maxHeight)
        {
            ImageSource = source.PhotoSource;
            MaxHeight = maxHeight * Math.Sqrt(2);
        }

        public void LoadNext(Action action)
        {
            action();
            LoadFullsizePhoto(++currentIndex);
        }

        public void LoadPrevious(Action action)
        {
            action();
            LoadFullsizePhoto(--currentIndex);
        }
    }
}
