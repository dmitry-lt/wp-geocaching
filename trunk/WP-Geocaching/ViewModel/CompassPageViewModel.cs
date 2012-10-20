using System;
using System.ComponentModel;
using WP_Geocaching.View.Compass;
using System.Device.Location;
using WP_Geocaching.Model.Utils;

namespace WP_Geocaching.ViewModel
{
    public class CompassPageViewModel : BaseViewModel, ICompassAware, INotifyPropertyChanged
    {
        private double northDirection;
        private double cacheDirection;
        private string azimuth;
        private double distance;
        private GeoCoordinate soughtPoint;
        private GeoCoordinate currentLocation;
        private double cacheAzimuth;
        private GeoCoordinateWatcher watcher;

        public string Azimuth
        {
            get { return azimuth; }
            set
            {
                azimuth = value;
                RaisePropertyChanged(() => Azimuth);
            }
        }

        public double NorthDirection
        {
            get { return northDirection; }
            set
            {
                northDirection = value;
                RaisePropertyChanged(() => NorthDirection);
            }
        }

        public double CacheDirection
        {
            get { return cacheDirection; }
            set
            {
                cacheDirection = value;
                RaisePropertyChanged(() => CacheDirection);
            }
        }

        public double Distance
        {
            get { return distance; }
            set
            {
                distance = value;
                RaisePropertyChanged(() => Distance);
            }
        }

        public GeoCoordinate CurrentLocation
        {
            get { return currentLocation; }
            set
            {
                currentLocation = value;
                RaisePropertyChanged(() => CurrentLocation);
            }
        }

        public GeoCoordinate SoughtPoint
        {
            get { return soughtPoint; }
            set
            {
                soughtPoint = value;
                RaisePropertyChanged(() => SoughtPoint);
            }
        }

        public CompassPageViewModel()
        {
            watcher = new GeoCoordinateWatcher();
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(PositionChanged);
            watcher.Start();
        }

        public void SetDirection(double direction)
        {
            NorthDirection = -direction;
            CacheDirection = NorthDirection + cacheAzimuth;
            double azimuth = (360 - NorthDirection % 360) % 360;
            Azimuth = String.Format("{0:F1}°", azimuth);
        }

        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            CurrentLocation = e.Position.Location;
            Distance = e.Position.Location.GetDistanceTo(SoughtPoint);
            cacheAzimuth = LocationHelper.CacheAzimuth(CurrentLocation, SoughtPoint);
        }
    }
}