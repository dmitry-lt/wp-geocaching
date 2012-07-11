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

namespace WP_Geocaching.Model
{
    public class ThreePhotos
    {
        public int Count { get; private set; }
        public ImageSource FirstPhotoSource { get; private set; }
        public ImageSource SecondPhotoSource { get; private set; }
        public ImageSource ThirdPhotoSource { get; private set; }

        public ThreePhotos()
        {
            Count = 0;
        }

        public void Add(ImageSource imageSource)
        {
            switch (Count)
            {
                case 0:
                    {
                        FirstPhotoSource = imageSource;
                        Count++;
                        return;
                    };
                case 1:
                    {
                        SecondPhotoSource = imageSource;
                        Count++;
                        return;
                    }
                case 2:
                    {
                        ThirdPhotoSource = imageSource;
                        Count++;
                        return;
                    }
            }
        }

        public bool isFull()
        {
            return (Count == 3);
        }
    }
}
