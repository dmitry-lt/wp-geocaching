using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using WP_Geocaching.Model;

namespace WP_Geocaching.ViewModel
{
    public class PhotoGalleryPageViewModel : BaseViewModel
    {
        private ImageSource imageSource;
        private int currentIndex;

        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource;
            }
            set
            {
                imageSource = value;
                NotifyPropertyChanged("ImageSource");
            }
        }

        public void LoadFullsizePhoto(int index)
        {
            currentIndex = index;
            GeocahingSuApiManager.Instance.ProcessPhoto(SetImageSource, index);
        }

        public void SetImageSource(ImageSource source)
        {
            ImageSource = source;
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
