using System.Windows;
using Microsoft.Phone.Controls;
using System.Device.Location;
using System.Windows.Navigation;
using System;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;
using System.Collections.Generic;

namespace WP_Geocaching.View.Compass
{
    public partial class CompassPage : PhoneApplicationPage
    {
        private CompassPageViewModal compassPageViewModal;
        private GeoCoordinateWatcher currentLocation;
        private GeoCoordinate soughtPoint;

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

            cacheDirectionLat = soughtPoint.Latitude > 0 ? 'N' : 'S';
            cacheDegreeLat = Math.Abs((int)soughtPoint.Latitude);
            cacheMinuteLat = (Math.Abs(soughtPoint.Latitude) - Math.Abs((int)soughtPoint.Latitude)) * 60;
            cacheDirectionLon = soughtPoint.Longitude > 0 ? 'E' : 'W';
            cacheDegreeLon = Math.Abs((int)soughtPoint.Longitude);
            cacheMinuteLon = (Math.Abs(soughtPoint.Longitude) - Math.Abs((int)soughtPoint.Longitude)) * 60;

            double y = Math.Sin((soughtPoint.Longitude - currentCoordinate.Longitude) * Math.PI / 180) * Math.Cos(soughtPoint.Latitude * Math.PI / 180);
            double x = Math.Cos(currentCoordinate.Latitude * Math.PI / 180) * Math.Sin(soughtPoint.Latitude * Math.PI / 180) -
                Math.Sin(currentCoordinate.Latitude * Math.PI / 180) * Math.Cos(soughtPoint.Latitude * Math.PI / 180) * Math.Cos((soughtPoint.Longitude - currentCoordinate.Longitude) * Math.PI / 180);
            double bearing = (Math.Atan2(y, x) * 180 / Math.PI + 360) % 360;

            double dir = (compassPageViewModal.Direction + 360) % 360;
            double cacheAzimuth = (bearing - dir + 360) % 360;
            compassPageViewModal.CacheAngle = cacheAzimuth + dir;

            curCoord.Text = String.Format("{0}° {1:F3}' {2} {3}° {4:F3}' {5}", currentDegreeLat, currentMinuteLat, currentDirectionLat, currentDegreeLon, currentMinuteLon, currentDirectionLon);
            cacheCoord.Text = String.Format("{0}° {1:F3}' {2} {3}° {4:F3}' {5}", cacheDegreeLat, cacheMinuteLat, cacheDirectionLat, cacheDegreeLon, cacheMinuteLon, cacheDirectionLon);
            distance.Text = String.Format("{0:F3} km", currentCoordinate.GetDistanceTo(soughtPoint) / 1000);
            azimuth.Text = String.Format("{0:F1}°", cacheAzimuth);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int currentId = Convert.ToInt32(NavigationContext.QueryString["CurrentId"]);
            int checkpointId = Convert.ToInt32(NavigationContext.QueryString["CheckpointId"]);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                CacheDataBase db = new CacheDataBase();
                soughtPoint = new GeoCoordinate();

                if (checkpointId != -1)
                {
                    List<DbCheckpointsItem> checkpoints = new List<DbCheckpointsItem>();
                    checkpoints = db.GetCheckpointsbyCacheId(currentId);

                    foreach (DbCheckpointsItem c in checkpoints)
                    {
                        if (c.Id == checkpointId)
                        {
                            soughtPoint.Latitude = c.Latitude;
                            soughtPoint.Longitude = c.Longitude;
                        }
                    }
                }
                else
                {
                    DbCacheItem cache = new DbCacheItem();
                    cache = db.GetCache(currentId);
                    soughtPoint.Latitude = cache.Latitude;
                    soughtPoint.Longitude = cache.Longitude;
                }
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