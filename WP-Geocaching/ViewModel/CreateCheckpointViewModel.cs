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
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.ViewModel
{
    public class CreateCheckpointViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string latitude;
        private string longitude;
        private string name;

        public string Latitude
        {
            get { return latitude; }
            set
            {
                bool changed = latitude != value;
                if (changed)
                {
                    latitude = value;
                    OnPropertyChanged("Latitude");
                }
            }
        }
        public string Longitude
        {
            get { return longitude; }
            set
            {
                bool changed = longitude != value;
                if (changed)
                {
                    longitude = value;
                    OnPropertyChanged("Longitude");
                }
            }
        }       
        public string Name
        {
            get { return name; }
            set
            {
                bool changed = name != value;
                if (changed)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public CreateCheckpointViewModel()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            this.Name = AppResources.NewCheckpoint;
            CacheDataBase db = new CacheDataBase();
            DbCacheItem cacheItem = db.GetCache(MapManager.Instance.CacheId);
            if (cacheItem != null)
            {
                this.Latitude = cacheItem.Latitude.ToString();
                this.Longitude = cacheItem.Longitude.ToString();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
