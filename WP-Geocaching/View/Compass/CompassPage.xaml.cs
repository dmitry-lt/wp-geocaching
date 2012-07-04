using System.Windows;
using Microsoft.Phone.Controls;
using System.Device.Location;
using System.Windows.Navigation;
using System;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.View.Compass
{
    public partial class CompassPage : PhoneApplicationPage
    {
        private CompassPageViewModal compassPageViewModal;
        private GeoCoordinateWatcher currentLocation;
        private GeoCoordinate soughtCache;

        private int currentDegreeLat;
        private double currentMinuteLat;
        private char currentDirectionLat;
        private int currentDegreeLon;
        private double currentMinuteLon;
        private char currentDirectionLon;

        private int cacheDegreeLat;
        private double cacheMinuteLat;
        private char cacheDirectionLat;
        private int cacheDegreeLon;
        private double cacheMinuteLon;
        private char cacheDirectionLon;

        private double cacheAzimuth;

        public CompassPage()
        {
            InitializeComponent();

            currentLocation = new GeoCoordinateWatcher();
            currentLocation.PositionChanged += new System.EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(currentLocation_PositionChanged);
            currentLocation.Start();
            compassPageViewModal = new CompassPageViewModal();
            DataContext = compassPageViewModal;
        }

        void currentLocation_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GeoCoordinate currentCoordinate = e.Position.Location;
            currentDirectionLat = currentCoordinate.Latitude > 0 ? 'N' : 'S';
            currentDegreeLat = Math.Abs((int)currentCoordinate.Latitude);
            currentMinuteLat = (Math.Abs(currentCoordinate.Latitude) - Math.Abs((int)currentCoordinate.Latitude)) * 60;
            currentDirectionLon = currentCoordinate.Longitude > 0 ? 'E' : 'W';
            currentDegreeLon = Math.Abs((int)currentCoordinate.Longitude);
            currentMinuteLon = (Math.Abs(currentCoordinate.Longitude) - Math.Abs((int)currentCoordinate.Longitude)) * 60;

            cacheDirectionLat = soughtCache.Latitude > 0 ? 'N' : 'S';
            cacheDegreeLat = Math.Abs((int)soughtCache.Latitude);
            cacheMinuteLat = (Math.Abs(soughtCache.Latitude) - Math.Abs((int)soughtCache.Latitude)) * 60;
            cacheDirectionLon = soughtCache.Longitude > 0 ? 'E' : 'W';
            cacheDegreeLon = Math.Abs((int)soughtCache.Longitude);
            cacheMinuteLon = (Math.Abs(soughtCache.Longitude) - Math.Abs((int)soughtCache.Longitude)) * 60;

            double y = Math.Sin((soughtCache.Longitude - currentCoordinate.Longitude) * Math.PI / 180) * Math.Cos(soughtCache.Latitude * Math.PI / 180);
            double x = Math.Cos(currentCoordinate.Latitude * Math.PI / 180) * Math.Sin(soughtCache.Latitude * Math.PI / 180) -
                Math.Sin(currentCoordinate.Latitude * Math.PI / 180) * Math.Cos(soughtCache.Latitude * Math.PI / 180) * Math.Cos((soughtCache.Longitude - currentCoordinate.Longitude) * Math.PI / 180);
            double bearing = Math.Atan2(y, x) * 180 / Math.PI;
            bearing = (bearing + 360) % 360;

            compassPageViewModal.CacheAngle = bearing;
            cacheAzimuth = (bearing - compassPageViewModal.Direction + 360) % 360;

            curCoord.Text = String.Format("{0}° {1:F3}' {2} {3}° {4:F3}' {5}", currentDegreeLat, currentMinuteLat, currentDirectionLat, currentDegreeLon, currentMinuteLon, currentDirectionLon);
            cacheCoord.Text = String.Format("{0}° {1:F3}' {2} {3}° {4:F3}' {5}", cacheDegreeLat, cacheMinuteLat, cacheDirectionLat, cacheDegreeLon, cacheMinuteLon, cacheDirectionLon);
            distance.Text = String.Format("{0:F2} km", currentCoordinate.GetDistanceTo(soughtCache) / 1000);
            azimuth.Text = String.Format("{0:F1}°", cacheAzimuth);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int soughtCacheId = Convert.ToInt32(NavigationContext.QueryString["CacheID"]);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                CacheDataBase db = new CacheDataBase();
                DbCacheItem cache = new DbCacheItem();

                cache = db.GetCache(soughtCacheId);
                soughtCache = new GeoCoordinate();
                soughtCache.Latitude = cache.Latitude;
                soughtCache.Longitude = cache.Longitude;
            }
        }

        private void LayoutRootLoaded(object sender, RoutedEventArgs e)
        {
            compassPageViewModal.Start();
        }

        //TODO: don't called on win-button click
        private void LayoutRootUnloaded(object sender, RoutedEventArgs e)
        {
            // Stop data acquisition from the compass.
            compassPageViewModal.Stop();
        }
    }
}