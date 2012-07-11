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

            curCoord.Text = String.Format("{0} {1}° {2:F3}' | {3} {4}° {5:F3}'",compassPageViewModal.CurrentDirectionLat, compassPageViewModal.CurrentDegreeLat, compassPageViewModal.CurrentMinuteLat, compassPageViewModal.CurrentDirectionLon, compassPageViewModal.CurrentDegreeLon, compassPageViewModal.CurrentMinuteLon);
            cacheCoord.Text = String.Format("{0} {1}° {2:F3}' | {3} {4}° {5:F3}'", compassPageViewModal.CacheDirectionLat, compassPageViewModal.CacheDegreeLat, compassPageViewModal.CacheMinuteLat, compassPageViewModal.CacheDirectionLon, compassPageViewModal.CacheDegreeLon, compassPageViewModal.CacheMinuteLon);
            distance.Text = String.Format("{0:F3} km", e.Position.Location.GetDistanceTo(soughtPoint) / 1000);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SmoothCompassManager.Instance.AddSubscriber(compassPageViewModal);
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SmoothCompassManager.Instance.RemoveSubscriber(compassPageViewModal);
        }
    }
}