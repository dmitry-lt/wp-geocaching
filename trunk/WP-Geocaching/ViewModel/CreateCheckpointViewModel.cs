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
using WP_Geocaching.Model.Utils;
using System.Device.Location;

namespace WP_Geocaching.ViewModel
{
    public class CreateCheckpointViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        private GeoCoordinate currentInputPointLocation;

        private string latDegrees;
        private string latMinutes;
        private string latMinutesFraction;
        private string lngDegrees;
        private string lngMinutes;
        private string lngMinutesFraction;

        private string dLatDegrees;
        private string dLatDegreesFraction;
        private string dLngDegrees;
        private string dLngDegreesFraction;

        public string Name
        {
            get { return name; }
            set
            {
                    name = value;               
            }
        }
        public GeoCoordinate CurrentInputPointLocation
        {
            get
            {
                return this.currentInputPointLocation;
            }
        }

        public string LatDegrees
        {
            get { return latDegrees; }
            set
            {
                   latDegrees = value;
               SexagesimalChanged();
            }
        }
        public string LatMinutes
        {
            get { return latMinutes; }
            set
            {            
                    latMinutes = value;
                    SexagesimalChanged();               
            }
        }
        public string LatMinutesFraction
        {
            get { return latMinutesFraction; }
            set
            {
                    latMinutesFraction = value;
                    SexagesimalChanged();
            }
        }
        public string LngDegrees
        {
            get { return lngDegrees; }
            set
            {
                    lngDegrees = value;
                    SexagesimalChanged();
            }
        }
        public string LngMinutes
        {
            get { return lngMinutes; }
            set
            {
                    lngMinutes = value;
                    SexagesimalChanged();               
            }
        }
        public string LngMinutesFraction
        {
            get { return lngMinutesFraction; }
            set
            {
                    lngMinutesFraction = value;
                    SexagesimalChanged();
            }
        }

        public string DLatDegrees
        {
            get { return dLatDegrees; }
            set
            {
                    dLatDegrees = value;
                    DecimalChanged();
            }
        }
        public string DLatDegreesFraction
        {
            get { return dLatDegreesFraction; }
            set
            {
                    dLatDegreesFraction = value;
                    DecimalChanged();
            }
        }
        public string DLngDegrees
        {
            get { return dLngDegrees; }
            set
            {
                    dLngDegrees = value;
                    DecimalChanged();
            }
        }
        public string DLngDegreesFraction
        {
            get { return dLngDegreesFraction; }
            set
            {
                    dLngDegreesFraction = value;
                    DecimalChanged();              
            }
        }


        public CreateCheckpointViewModel()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            name = AppResources.NewCheckpoint;
            CacheDataBase db = new CacheDataBase();
            DbCacheItem cacheItem = db.GetCache(MapManager.Instance.CacheId);
            updateTextBoxes();
        }

        private void updateTextBoxes()
        {          
            if (currentInputPointLocation == null)
            {
                CacheDataBase db = new CacheDataBase();
                DbCacheItem cacheItem = db.GetCache(MapManager.Instance.CacheId);
                currentInputPointLocation = new GeoCoordinate (cacheItem.Latitude, cacheItem.Longitude);
            }
            updateDecimal();
            updateSexagesimal();
        }

        private void updateSexagesimal()
        {
            double lat = currentInputPointLocation.Latitude;
            double lng = currentInputPointLocation.Longitude;

            Sexagesimal sexagesimal = new Sexagesimal(lat).roundTo(3);
            latDegrees = sexagesimal.Degrees.ToString();
            int minutesE3 = (int)Math.Round(sexagesimal.Minutes * 1000);
            latMinutes = (minutesE3 / 1000).ToString();
            latMinutesFraction = (minutesE3 % 1000).ToString();

            sexagesimal = new Sexagesimal(lng).roundTo(3);
            lngDegrees = sexagesimal.Degrees.ToString();
            minutesE3 = (int)Math.Round(sexagesimal.Minutes * 1000);
            lngMinutes = (minutesE3 / 1000).ToString();
            lngMinutesFraction = (minutesE3 % 1000).ToString();

            OnPropertyChanged("LatDegrees");
            OnPropertyChanged("LatMinutes");
            OnPropertyChanged("LatMinutesFraction");
            OnPropertyChanged("LngDegrees");
            OnPropertyChanged("LngMinutes");
            OnPropertyChanged("LngMinutesFraction");
        }

        private void updateDecimal()
        {
            double lat = currentInputPointLocation.Latitude;
            double lng = currentInputPointLocation.Longitude;

            dLatDegrees = ((int)lat).ToString();
            dLngDegrees = ((int)lng).ToString();
            dLatDegreesFraction = ((int)((lat - (int)lat) * 1000000)).ToString();
            dLngDegreesFraction = ((int)((lng - (int)lng) * 1000000)).ToString();

            OnPropertyChanged("DLatDegrees");
            OnPropertyChanged("DLngDegrees");
            OnPropertyChanged("DLatDegreesFraction");
            OnPropertyChanged("DLngDegreesFraction");
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SexagesimalChanged()
        {
            try
            {
                int degreesInt = Convert.ToInt32(latDegrees);
                int minutesInt = Convert.ToInt32(latMinutes);
                double minutesFloat = Convert.ToDouble("." + latMinutesFraction);
                double latitude = new Sexagesimal(degreesInt, (double)minutesInt + minutesFloat).toCoordinate();

                degreesInt = Convert.ToInt32(lngDegrees);
                minutesInt = Convert.ToInt32(lngMinutes);
                minutesFloat = Convert.ToDouble("." + lngMinutesFraction);
                double longitude = new Sexagesimal(degreesInt, (double)minutesInt + minutesFloat).toCoordinate();

                currentInputPointLocation = new GeoCoordinate(latitude, longitude);
                updateDecimal();
            }
            catch (Exception)
            {
                //TODO: Message
            }
        }

        public void DecimalChanged()
        {
            try
            {
                double latitude = (double)(Convert.ToInt32(dLatDegrees) + Convert.ToDouble("." + dLatDegreesFraction));
                double longitude = (double)(Convert.ToInt32(dLngDegrees) + Convert.ToDouble("." + dLngDegreesFraction));
                currentInputPointLocation = new GeoCoordinate(latitude, longitude);
                updateSexagesimal();
            }
            catch (Exception)
            {
                //TODO: Message
            }
        }
    }
}
