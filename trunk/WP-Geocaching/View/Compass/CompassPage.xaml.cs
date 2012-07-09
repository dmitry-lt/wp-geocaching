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

        public CompassPage()
        {
            InitializeComponent();

            currentLocation = new GeoCoordinateWatcher();
            currentLocation.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(currentLocation_PositionChanged);
            currentLocation.Start();
            compassPageViewModal = new CompassPageViewModal();
            DataContext = compassPageViewModal;
        }

        void currentLocation_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            compassPageViewModal.CalculateBearing(e.Position.Location);

            curCoord.Text = String.Format("{0}° {1:F3}' {2} {3}° {4:F3}' {5}", compassPageViewModal.CurrentDegreeLat, compassPageViewModal.CurrentMinuteLat, compassPageViewModal.CurrentDirectionLat, compassPageViewModal.CurrentDegreeLon, compassPageViewModal.CurrentMinuteLon, compassPageViewModal.CurrentDirectionLon);
            cacheCoord.Text = String.Format("{0}° {1:F3}' {2} {3}° {4:F3}' {5}", compassPageViewModal.CacheDegreeLat, compassPageViewModal.CacheMinuteLat, compassPageViewModal.CacheDirectionLat, compassPageViewModal.CacheDegreeLon, compassPageViewModal.CacheMinuteLon, compassPageViewModal.CacheDirectionLon);
            distance.Text = String.Format("{0:F3} km", e.Position.Location.GetDistanceTo(soughtPoint) / 1000);
            azimuth.Text = String.Format("{0:F1}°", compassPageViewModal.Bearing);
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

                            compassPageViewModal.SoughtPoint = soughtPoint;
                        }
                    }
                }
                else
                {
                    DbCacheItem cache = new DbCacheItem();
                    cache = db.GetCache(currentId);
                    soughtPoint.Latitude = cache.Latitude;
                    soughtPoint.Longitude = cache.Longitude;

                    compassPageViewModal.SoughtPoint = soughtPoint;
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