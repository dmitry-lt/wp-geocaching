using System.Windows.Media;
using System.ComponentModel;
using WP_Geocaching.Model.Utils;

namespace WP_Geocaching.Model
{
    public class Photo : INotifyPropertyChanged
    {
        private ImageSource photoSource;

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
                PropertyChanged.Raise(() => PhotoSource);
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool IsPlaceholder { get; set; }

        public string PhotoName { get; set; }

        public Photo(ImageSource source, string photoName, bool isPlaceholder)
        {
            PhotoSource = source;
            PhotoName = photoName;
            IsPlaceholder = isPlaceholder;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Photo)) return false;
            return Equals((Photo)obj);
        }

        public bool Equals(Photo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.photoSource, photoSource);
        }

        public override int GetHashCode()
        {
            return (photoSource != null ? photoSource.GetHashCode() : 0);
        }
    }
}
