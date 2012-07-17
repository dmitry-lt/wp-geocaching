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
using System.Globalization;

namespace WP_Geocaching.ViewModel
{
    public class CreateCheckpointViewModel : BaseViewModel
    {
        private string name;
        private GeoCoordinate currentInputPointLocation;

        private string sLatDegrees;
        private string sLatMinutes;
        private string sLatSeconds;
        private string sLngDegrees;
        private string sLngMinutes;
        private string sLngSeconds;

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
                NotifyPropertyChanged("Name");
            }
        }
        public GeoCoordinate CurrentInputPointLocation
        {
            get
            {
                return this.currentInputPointLocation;
            }
        }

        public string SLatDegrees
        {
            get { return sLatDegrees; }
            set
            {
                sLatDegrees = value;
                SexagesimalSecondsChanged();
            }
        }
        public string SLatMinutes
        {
            get { return sLatMinutes; }
            set
            {
                sLatMinutes = value;
                SexagesimalSecondsChanged();
            }
        }
        public string SLatSeconds
        {
            get { return sLatSeconds; }
            set
            {
                sLatSeconds = value;
                SexagesimalSecondsChanged();
            }
        }
        public string SLngDegrees
        {
            get { return sLngDegrees; }
            set
            {
                sLngDegrees = value;
                SexagesimalSecondsChanged();

            }
        }
        public string SLngMinutes
        {
            get { return sLngMinutes; }
            set
            {
                sLngMinutes = value;
                SexagesimalSecondsChanged();
            }
        }
        public string SLngSeconds
        {
            get { return sLngSeconds; }
            set
            {
                sLngSeconds = value;
                SexagesimalSecondsChanged();
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
            CacheDataBase db = new CacheDataBase();
            name = String.Format(AppResources.DefaultCheckpointName, db.GetMaxCheckpointId(MapManager.Instance.CacheId) + 1);
            DbCacheItem cacheItem = db.GetCache(MapManager.Instance.CacheId);
            updateTextBoxes();
        }

        private void updateTextBoxes()
        {
            if (currentInputPointLocation == null)
            {
                CacheDataBase db = new CacheDataBase();
                DbCacheItem cacheItem = db.GetCache(MapManager.Instance.CacheId);
                currentInputPointLocation = new GeoCoordinate(cacheItem.Latitude, cacheItem.Longitude);
            }
            updateDecimal();
            updateSexagesimal();
            updateSexagesimalSeconds();
        }

        private void updateSexagesimalSeconds()
        {
            double lat = Convert.ToDouble(currentInputPointLocation.Latitude, CultureInfo.InvariantCulture);
            double lng = Convert.ToDouble(currentInputPointLocation.Longitude, CultureInfo.InvariantCulture);

            SexagesimalSec sSexagesimal = new SexagesimalSec(lat).roundTo(2);
            sLatDegrees = sSexagesimal.degrees.ToString();
            sLatMinutes = sSexagesimal.minutes.ToString();
            sLatSeconds = sSexagesimal.seconds.ToString();

            sSexagesimal = new SexagesimalSec(lng).roundTo(2);
            sLngDegrees = sSexagesimal.degrees.ToString();
            sLngMinutes = sSexagesimal.minutes.ToString();
            sLngSeconds = sSexagesimal.seconds.ToString();

            NotifyPropertyChanged("SLatDegrees");
            NotifyPropertyChanged("SLatMinutes");
            NotifyPropertyChanged("SLatSeconds");
            NotifyPropertyChanged("SLngDegrees");
            NotifyPropertyChanged("SLngMinutes");
            NotifyPropertyChanged("SLngSeconds");
        }

        private void updateSexagesimal()
        {
            double lat = Convert.ToDouble(currentInputPointLocation.Latitude, CultureInfo.InvariantCulture);
            double lng = Convert.ToDouble(currentInputPointLocation.Longitude, CultureInfo.InvariantCulture);

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

            NotifyPropertyChanged("LatDegrees");
            NotifyPropertyChanged("LatMinutes");
            NotifyPropertyChanged("LatMinutesFraction");
            NotifyPropertyChanged("LngDegrees");
            NotifyPropertyChanged("LngMinutes");
            NotifyPropertyChanged("LngMinutesFraction");
        }

        private void updateDecimal()
        {
            double lat = Convert.ToDouble(currentInputPointLocation.Latitude, CultureInfo.InvariantCulture);
            double lng = Convert.ToDouble(currentInputPointLocation.Longitude, CultureInfo.InvariantCulture);

            dLatDegrees = ((int)lat).ToString();
            dLngDegrees = ((int)lng).ToString();
            dLatDegreesFraction = ((int)((lat - (int)lat) * 1000000)).ToString();
            dLngDegreesFraction = ((int)((lng - (int)lng) * 1000000)).ToString();

            NotifyPropertyChanged("DLatDegrees");
            NotifyPropertyChanged("DLngDegrees");
            NotifyPropertyChanged("DLatDegreesFraction");
            NotifyPropertyChanged("DLngDegreesFraction");
        }

        public void SexagesimalSecondsChanged()
        {
            try
            {
                int degreesInt = Convert.ToInt32(sLatDegrees, CultureInfo.InvariantCulture);
                int minutesInt = Convert.ToInt32(sLatMinutes, CultureInfo.InvariantCulture);
                double secondsDouble = Convert.ToDouble(sLatSeconds, CultureInfo.InvariantCulture);
                double latitude = new SexagesimalSec(degreesInt, minutesInt, secondsDouble).toCoordinate();

                degreesInt = Convert.ToInt32(sLngDegrees, CultureInfo.InvariantCulture);
                minutesInt = Convert.ToInt32(sLngMinutes, CultureInfo.InvariantCulture);
                secondsDouble = Convert.ToDouble(sLngSeconds, CultureInfo.InvariantCulture);
                double longitude = new SexagesimalSec(degreesInt, minutesInt, secondsDouble).toCoordinate();

                currentInputPointLocation = new GeoCoordinate(latitude, longitude);
                updateSexagesimal();
                updateDecimal();
            }
            catch (Exception)
            {
                //TODO: Message
            }
        }

        public void SexagesimalChanged()
        {
            try
            {
                int degreesInt = Convert.ToInt32(latDegrees, CultureInfo.InvariantCulture);
                int minutesInt = Convert.ToInt32(latMinutes, CultureInfo.InvariantCulture);
                double minutesDouble = Convert.ToDouble("." + latMinutesFraction, CultureInfo.InvariantCulture);
                double latitude = new Sexagesimal(degreesInt, (double)minutesInt + minutesDouble).toCoordinate();

                degreesInt = Convert.ToInt32(lngDegrees, CultureInfo.InvariantCulture);
                minutesInt = Convert.ToInt32(lngMinutes, CultureInfo.InvariantCulture);
                minutesDouble = Convert.ToDouble("." + lngMinutesFraction, CultureInfo.InvariantCulture);
                double longitude = new Sexagesimal(degreesInt, (double)minutesInt + minutesDouble).toCoordinate();

                currentInputPointLocation = new GeoCoordinate(latitude, longitude);
                updateDecimal();
                updateSexagesimalSeconds();
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
                double latitude = (double)(Convert.ToInt32(dLatDegrees, CultureInfo.InvariantCulture) + Convert.ToDouble("." + dLatDegreesFraction, CultureInfo.InvariantCulture));
                double longitude = (double)(Convert.ToInt32(dLngDegrees, CultureInfo.InvariantCulture) + Convert.ToDouble("." + dLngDegreesFraction, CultureInfo.InvariantCulture));
                currentInputPointLocation = new GeoCoordinate(latitude, longitude);
                updateSexagesimal();
                updateSexagesimalSeconds();
            }
            catch (Exception)
            {
                //TODO: Message
            }
        }

        public void SavePoint()
        {
            CacheDataBase db = new CacheDataBase();
            db.AddActiveCheckpoint(MapManager.Instance.CacheId, Name,
                CurrentInputPointLocation.Latitude,
                CurrentInputPointLocation.Longitude);
        }
    }
}