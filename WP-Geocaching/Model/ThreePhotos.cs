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
        public int FirstPhotoIndex { get; private set; }
        public ImageSource SecondPhotoSource { get; private set; }
        public int SecondPhotoIndex { get; private set; }
        public ImageSource ThirdPhotoSource { get; private set; }
        public int ThirdPhotoIndex { get; private set; }

        public ThreePhotos()
        {
            Count = 0;
        }

        public void Add(ImageSource imageSource, int index)
        {
            switch (Count)
            {
                case 0:
                    {
                        FirstPhotoSource = imageSource;
                        FirstPhotoIndex = index;
                        Count++;
                        return;
                    };
                case 1:
                    {
                        SecondPhotoSource = imageSource;
                        SecondPhotoIndex = index;
                        Count++;
                        return;
                    }
                case 2:
                    {
                        ThirdPhotoSource = imageSource;
                        ThirdPhotoIndex = index;
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
