using System;
using System.Windows;
using WP_Geocaching.Model;
using System.Collections.Generic;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;

namespace WP_Geocaching.ViewModel
{
    public class BingMapViewModel : BaseMapViewModel, ILocationAware
    {

        private const int maxCacheCount = 50;
        private LocationRect boundingRectangle;
        private Visibility surpassedCacheCountMessageVisibility = Visibility.Collapsed;
        private GeoCoordinateWatcher watcher;
        private bool isFirstSettingView;

        public Visibility SurpassedCacheCountMessageVisibility
        {
            get
            {
                return surpassedCacheCountMessageVisibility;
            }
            set
            {
                surpassedCacheCountMessageVisibility = value;
                NotifyPropertyChanged("SurpassedCacheCountMessageVisibility");
            }
        }

        public LocationRect BoundingRectangle
        {
            get
            {
                return boundingRectangle;
            }
            set
            {
                boundingRectangle = value;
                SetPushpinsOnMap();
            }
        }

        public bool IsNeedHighAccuracy
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public BingMapViewModel(IApiManager apiManager)
        {
            var settings = new Settings();
            MapCenter = settings.LastLocation;
            MapMode = settings.MapMode;
            Zoom = MapManager.Instance.DefaultZoom;
            this.apiManager = apiManager;
            CachePushpins = new ObservableCollection<CachePushpin>();
            isFirstSettingView = true;
        }

        public void UpdateMapChildrens()
        {
            UpdateMapMode();
        }

        private void ProcessCacheList(List<Cache> caches)
        {

            CachePushpins.Clear();

            if (caches.Count >= maxCacheCount)
            {

                if (surpassedCacheCountMessageVisibility.Equals(Visibility.Collapsed))
                {
                    SurpassedCacheCountMessageVisibility = Visibility.Visible;
                }
            }
            else
            {

                foreach (var p in caches)
                {
                    var pushpin = new CachePushpin()
                    {
                        Location = p.Location,
                        Id = p.Id.ToString(),
                        IconUri = new Enum[2] { p.Type, p.Subtype }
                    };

                    CachePushpins.Add(pushpin);
                }

                if (surpassedCacheCountMessageVisibility.Equals(Visibility.Visible))
                {
                    SurpassedCacheCountMessageVisibility = Visibility.Collapsed;
                }
            }
        }

        private void SetPushpinsOnMap()
        {
            apiManager.UpdateCacheList(ProcessCacheList, BoundingRectangle.East,
                BoundingRectangle.West, BoundingRectangle.North, BoundingRectangle.South);
        }
        
        public void ProcessLocation(GeoCoordinate location)
        {
            var settings = new Settings();
            var currentLocation = location;
            settings.LastLocation = currentLocation;
            this.currentLocation = currentLocation;
            if (!isFirstSettingView)
            {
                return;
            }
            MapCenter = currentLocation;
            isFirstSettingView = false;
        }
    }
}
