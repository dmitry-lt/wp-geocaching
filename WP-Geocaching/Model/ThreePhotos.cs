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
using System.ComponentModel;

namespace WP_Geocaching.Model
{
    public class ThreePhotos : INotifyPropertyChanged
    {
        private ImageSource firstPhotoSource;
        private int firstPhotoIndex;
        private ImageSource secondPhotoSource;
        private int secondPhotoIndex;
        private ImageSource thirdPhotoSource;
        private int thirdPhotoIndex;

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageSource FirstPhotoSource 
        {
            get
            {
                return firstPhotoSource;
            }
            private set
            {
                firstPhotoSource = value;
                NotifyPropertyChanged("FirstPhotoSource");
            }
        }

        public int FirstPhotoIndex
        {
            get
            {
                return firstPhotoIndex;
            }
            private set
            {
                firstPhotoIndex = value;
                NotifyPropertyChanged("FirstPhotoIndex");
            }
        }

        public ImageSource SecondPhotoSource
        {
            get
            {
                return secondPhotoSource;
            }
            private set
            {
                secondPhotoSource = value;
                NotifyPropertyChanged("SecondPhotoSource");
            }
        }

        public int SecondPhotoIndex
        {
            get
            {
                return secondPhotoIndex;
            }
            private set
            {
                secondPhotoIndex = value;
                NotifyPropertyChanged("SecondPhotoIndex");
            }
        }

        public ImageSource ThirdPhotoSource
        {
            get
            {
                return thirdPhotoSource;
            }
            private set
            {
                thirdPhotoSource = value;
                NotifyPropertyChanged("ThirdPhotoSource");
            }
        }

        public int ThirdPhotoIndex
        {
            get
            {
                return thirdPhotoIndex;
            }
            private set
            {
                thirdPhotoIndex = value;
                NotifyPropertyChanged("ThirdPhotoIndex");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Add(ImageSource imageSource, int index)
        {
            switch (index % 3)
            {
                case 0:
                    {
                        FirstPhotoSource = imageSource;
                        FirstPhotoIndex = index;
                        return;
                    };
                case 1:
                    {
                        SecondPhotoSource = imageSource;
                        SecondPhotoIndex = index;
                        return;
                    }
                case 2:
                    {
                        ThirdPhotoSource = imageSource;
                        ThirdPhotoIndex = index;
                        return;
                    }
            }
        }
    }
}
