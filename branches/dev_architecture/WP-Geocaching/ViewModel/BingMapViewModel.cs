using System;
using System.Windows;
using WP_Geocaching.Model;
using System.Collections.Generic;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;
using WP_Geocaching.Model.Api;

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

        private Dictionary<Cache, CachePushpin> _currentPushpins = new Dictionary<Cache, CachePushpin>();
        private object _lock = new object();

        private void AddPushpin(Cache cache)
        {
            var pushpin = new CachePushpin()
            {
                Location = cache.Location,
                Id = cache.Id,
                CacheProvider = cache.CacheProvider,
                IconUri = new Enum[2] { cache.Type, cache.Subtype }
            };

            _currentPushpins.Add(cache, pushpin);

            CachePushpins.Add(pushpin);
        }

        private void RemovePushpin(Cache cache)
        {
            var pushpin = _currentPushpins[cache];
            
            _currentPushpins.Remove(cache);

            CachePushpins.Remove(pushpin);
        }

        // TODO: refactor
        private void ProcessCaches(List<Cache> caches)
        {
            lock (_lock)
            {
                // Remove pushpins that are out of screen
                var cachesToRemove = new HashSet<Cache>();
                foreach (var c in _currentPushpins.Keys)
                {
                    if ((c.Location.Latitude > BoundingRectangle.North) ||
                        (c.Location.Latitude < BoundingRectangle.South) ||
                        (c.Location.Longitude > BoundingRectangle.East) ||
                        (c.Location.Longitude < BoundingRectangle.West))
                    {
                        cachesToRemove.Add(c);
                    }
                }

                foreach (var c in cachesToRemove)
                {
                    RemovePushpin((Cache)c);
                }

                var cachesToAdd = new HashSet<Cache>();
                foreach (var c in caches)
                {
                    if (!_currentPushpins.ContainsKey(c))
                    {
                        cachesToAdd.Add(c);
                    }
                }

                if (_currentPushpins.Count + cachesToAdd.Count >= maxCacheCount)
                {
                    if (surpassedCacheCountMessageVisibility.Equals(Visibility.Collapsed))
                    {
                        SurpassedCacheCountMessageVisibility = Visibility.Visible;
                    }
                }
                else
                {

                    foreach (var c in cachesToAdd)
                    {
                        AddPushpin((Cache) c);
                    }

                    if (surpassedCacheCountMessageVisibility.Equals(Visibility.Visible))
                    {
                        SurpassedCacheCountMessageVisibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void SetPushpinsOnMap()
        {
            apiManager.UpdateCaches(ProcessCaches, BoundingRectangle.East,
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
