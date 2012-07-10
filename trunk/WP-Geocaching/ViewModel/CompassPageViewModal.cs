using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using WP_Geocaching.Model;
using WP_Geocaching.View.Compass;
using System.Device.Location;

namespace WP_Geocaching.ViewModel
{
    public class CompassPageViewModal : BaseViewModel, ICompassView
    {
        //private readonly SmoothCompassManager smoothCompassManager;
        private double _northDirection;
        private double _cacheAngle;

        public int CurrentDegreeLat { get; set; }

        public double CurrentMinuteLat { get; set; }

        public char CurrentDirectionLat { get; set; }

        public int CurrentDegreeLon { get; set; }

        public double CurrentMinuteLon { get; set; }

        public char CurrentDirectionLon { get; set; }

        public int CacheDegreeLat { get; set; }

        public double CacheMinuteLat { get; set; }

        public char CacheDirectionLat { get; set; }

        public int CacheDegreeLon { get; set; }

        public double CacheMinuteLon { get; set; }

        public char CacheDirectionLon { get; set; }

        public double CacheAzimuth { get; set; }

        public double Azimuth { get; set; }

        public GeoCoordinate SoughtPoint { get; set; }

        public double NorthDirection
        {
            get { return _northDirection; }
            set
            {
                _northDirection = value;
                NotifyPropertyChanged("NorthDirection");
            }
        }

        public double CacheAngle
        {
            get { return _cacheAngle; }
            set
            {
                _cacheAngle = value;
                NotifyPropertyChanged("CacheAngle");
            }
        }

        public CompassPageViewModal()
        {
            //smoothCompassManager = new SmoothCompassManager(this);
            SmoothCompassManager.Instance.AddSubscriber(this);
        }

        private DateTime time;
        public void SetDirection(double direction)
        {
            NorthDirection = -direction;
            CacheAngle = NorthDirection + CacheAzimuth;
            Debug.WriteLine("fps " + 1000 / (DateTime.Now - time).Milliseconds);
            time = DateTime.Now;
        }

        public void Start()
        {
            //smoothCompassManager.Start();
            SmoothCompassManager.Instance.Start();
        }

        public void Stop()
        {
            //smoothCompassManager.Stop();
            SmoothCompassManager.Instance.Stop();
        }

        public void CalculateBearing(GeoCoordinate currentCoordinate)
        {
            CurrentDirectionLat = currentCoordinate.Latitude > 0 ? 'N' : 'S';
            CurrentDegreeLat = Math.Abs((int)currentCoordinate.Latitude);
            CurrentMinuteLat = (Math.Abs(currentCoordinate.Latitude) - Math.Abs((int)currentCoordinate.Latitude)) * 60;
            CurrentDirectionLon = currentCoordinate.Longitude > 0 ? 'E' : 'W';
            CurrentDegreeLon = Math.Abs((int)currentCoordinate.Longitude);
            CurrentMinuteLon = (Math.Abs(currentCoordinate.Longitude) - Math.Abs((int)currentCoordinate.Longitude)) * 60;

            CacheDirectionLat = SoughtPoint.Latitude > 0 ? 'N' : 'S';
            CacheDegreeLat = Math.Abs((int)SoughtPoint.Latitude);
            CacheMinuteLat = (Math.Abs(SoughtPoint.Latitude) - Math.Abs((int)SoughtPoint.Latitude)) * 60;
            CacheDirectionLon = SoughtPoint.Longitude > 0 ? 'E' : 'W';
            CacheDegreeLon = Math.Abs((int)SoughtPoint.Longitude);
            CacheMinuteLon = (Math.Abs(SoughtPoint.Longitude) - Math.Abs((int)SoughtPoint.Longitude)) * 60;

            double y = Math.Sin((SoughtPoint.Longitude - currentCoordinate.Longitude) * Math.PI / 180) * Math.Cos(SoughtPoint.Latitude * Math.PI / 180);
            double x = Math.Cos(currentCoordinate.Latitude * Math.PI / 180) * Math.Sin(SoughtPoint.Latitude * Math.PI / 180) -
                Math.Sin(currentCoordinate.Latitude * Math.PI / 180) * Math.Cos(SoughtPoint.Latitude * Math.PI / 180) * Math.Cos((SoughtPoint.Longitude - currentCoordinate.Longitude) * Math.PI / 180);
            CacheAzimuth = (Math.Atan2(y, x) * 180 / Math.PI + 360) % 360;

            Azimuth = (360 - NorthDirection) % 360;
        }
    }
}