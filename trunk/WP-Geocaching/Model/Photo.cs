using System.Windows.Media;
using System.ComponentModel;

namespace WP_Geocaching.Model
{
    public class Photo : INotifyPropertyChanged
    {
        private ImageSource photoSource;
        private int index;

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageSource PhotoSource 
        {
            get
            {
                return photoSource;
            }
            set
            {
                photoSource = value;
                NotifyPropertyChanged("PhotoSource");
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
                NotifyPropertyChanged("Index");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
