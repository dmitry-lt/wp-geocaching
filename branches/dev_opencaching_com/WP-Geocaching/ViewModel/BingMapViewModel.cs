﻿using System;
using System.Windows;
using WP_Geocaching.Model;
using System.Collections.Generic;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Api.GeocachingSu;

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

        private readonly HashSet<Cache> _allCaches = new HashSet<Cache>();
        private readonly Dictionary<Cache, CachePushpin> _currentPushpins = new Dictionary<Cache, CachePushpin>();
        private readonly object _lock = new object();

        private void AddPushpin(Cache cache)
        {
            var pushpin = new CachePushpin()
            {
                Cache = cache
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

        private void RemoveAllPushpins()
        {
            _currentPushpins.Clear();
            CachePushpins.Clear();
        }

        private void ProcessCaches(List<Cache> caches)
        {
            lock (_lock)
            {
                if (null != caches)
                {
                    foreach (var c in caches)
                    {
                        if (!_allCaches.Contains(c))
                        {
                            _allCaches.Add(c);
                        }
                    }
                }

                var cachesOnScreen = new HashSet<Cache>();
                foreach (Cache c in _allCaches)
                {
                    if ((c.Location.Latitude <= BoundingRectangle.North) &&
                        (c.Location.Latitude >= BoundingRectangle.South) &&
                        (c.Location.Longitude <= BoundingRectangle.East) &&
                        (c.Location.Longitude >= BoundingRectangle.West))
                    {
                        cachesOnScreen.Add(c);
                    }
                }

                if (cachesOnScreen.Count >= maxCacheCount)
                {
                    if (surpassedCacheCountMessageVisibility.Equals(Visibility.Collapsed))
                    {
                        SurpassedCacheCountMessageVisibility = Visibility.Visible;
                    }
                    RemoveAllPushpins();
                }
                else
                {
                    if (surpassedCacheCountMessageVisibility.Equals(Visibility.Visible))
                    {
                        SurpassedCacheCountMessageVisibility = Visibility.Collapsed;
                    }

                    var cachesToRemove = new HashSet<Cache>();
                    foreach (var c in _currentPushpins.Keys)
                    {
                        if (!cachesOnScreen.Contains(c))
                        {
                            cachesToRemove.Add(c);
                        }
                    }

                    foreach (Cache c in cachesToRemove)
                    {
                        RemovePushpin(c);
                    }

                    foreach (Cache c in cachesOnScreen)
                    {
                        if (!_currentPushpins.ContainsKey(c))
                        {
                            AddPushpin((Cache) c);
                        }
                    }
                }
            }
        }

        private void SetPushpinsOnMap()
        {
            ProcessCaches(null);
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
