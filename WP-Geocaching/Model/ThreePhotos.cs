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
        public PreviewImage FirstPhoto { get; private set; }
        public PreviewImage SecondPhoto { get; private set; }
        public PreviewImage ThirdPhoto { get; private set; }

        public ThreePhotos()
        {
            Count = 0;
        }

        public void Add(PreviewImage image)
        {
            switch (Count)
            {
                case 0:
                    {
                        FirstPhoto = image;
                        Count++;
                        return;
                    };
                case 1:
                    {
                        SecondPhoto = image;
                        Count++;
                        return;
                    }
                case 2:
                    {
                        ThirdPhoto = image;
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
