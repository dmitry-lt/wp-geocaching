using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Utils;

namespace GeocachingPlus.ViewModel
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
                RaisePropertyChanged(() => ImageSource);
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
                RaisePropertyChanged(() => MaxHeight);
            }
        }

        public List<Photo> Photos { get; set; }

        public void LoadFullsizePhoto(int index)
        {
            while (index < 0)
            {
                index += Photos.Count;
            }
            index %= Photos.Count;

            var source = (BitmapSource)Photos[index].PhotoSource;

            currentIndex = index;
            SetImageSource(source);
        }

        public void SetImageSource(BitmapSource source)
        {
            ImageSource = source;
            MaxHeight = source.PixelHeight * Math.Sqrt(2);
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
