﻿using System.Windows;
using GeocachingPlus.Model;
using System.Collections.Generic;
using System.Device.Location;
using System.Collections.ObjectModel;
using GeocachingPlus.Model.Api.GeocachingCom;
using GeocachingPlus.Model.DataBase;
using Microsoft.Phone.Controls.Maps;
using GeocachingPlus.Model.Api;

namespace GeocachingPlus.ViewModel
{
    public class BingMapViewModel : BaseMapViewModel, ILocationAware
    {
        private const int maxCacheCount = 50;
        private LocationRect boundingRectangle;
        private Visibility surpassedCacheCountMessageVisibility = Visibility.Collapsed;

        public Visibility SurpassedCacheCountMessageVisibility
        {
            get
            {
                return surpassedCacheCountMessageVisibility;
            }
            set
            {
                surpassedCacheCountMessageVisibility = value;
                RaisePropertyChanged(() => SurpassedCacheCountMessageVisibility);
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
        }

        public void ProcessLocation(GeoCoordinate location)
        {
            currentLocation = location;
        }

        private GeoCoordinate _mapCenter;
        public override GeoCoordinate MapCenter
        {
            get
            {
                return _mapCenter;
            }
            set
            {
                _mapCenter = value;
                RaisePropertyChanged(() => MapCenter);
            }
        }

        private int _zoom;
        public override int Zoom 
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
                RaisePropertyChanged(() => Zoom);
            }
        }

        public RequestCounter RequestCounter { get { return RequestCounter.LiveMap; } }

        public BingMapViewModel(IApiManager apiManager)
        {
            var settings = new Settings();
            MapMode = settings.MapMode;
            MapCenter = settings.LatestChooseLocation;
            Zoom = settings.LatestChooseZoom;
            this.apiManager = apiManager;
            CachePushpins = new ObservableCollection<CachePushpin>();
        }

        public void UpdateMapChildrens()
        {
            UpdateMapMode();
        }

        private readonly HashSet<Cache> _allCaches = new HashSet<Cache>();
        private readonly Dictionary<Cache, CachePushpin> _currentPushpins = new Dictionary<Cache, CachePushpin>();
        private readonly object _lock = new object();

        private Dictionary<string, GeocachingComLookupInstance> _geocachingComLookupDictionary;

        private void InitGeocachingComLookupDictionary()
        {
            if (null == _geocachingComLookupDictionary)
            {
                _geocachingComLookupDictionary = new CacheDataBase().GetGeocachingComLookupDictionary();
            }
        }

        private void CheckForMoreCacheDetailsInDb(Cache cache)
        {
            if (cache is GeocachingComCache)
            {
                InitGeocachingComLookupDictionary();
                var gcCache = cache as GeocachingComCache;
                
                // check for type and reliable location
                if (_geocachingComLookupDictionary.ContainsKey(gcCache.Id))
                {
                    var lookup = _geocachingComLookupDictionary[gcCache.Id];
                    if (gcCache.Type == GeocachingComCache.Types.UNKNOWN)
                    {
                        gcCache.Type = lookup.Type;
                    }
                    if (!gcCache.ReliableLocation && lookup.ReliableLocation)
                    {
                        gcCache.Location = lookup.Location;
                        gcCache.ReliableLocation = lookup.ReliableLocation;
                    }
                }
            }
        }

        private void AddPushpin(Cache cache)
        {
            CheckForMoreCacheDetailsInDb(cache);

            var pushpin = new CachePushpin()
            {
                CacheInfo = cache
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

        private bool _tooManyCachesOnScreen;

        public int AllCachesCount { get { return _allCaches.Count; } }

        private void ProcessCaches(FetchCaches caches)
        {
            lock (_lock)
            {
                if (null != caches)
                {
                    foreach (var c in caches.Caches)
                    {
                        if (!_allCaches.Contains(c))
                        {
                            _allCaches.Add(c);
                        }
                    }
                    RaisePropertyChanged(() => AllCachesCount);
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

                _tooManyCachesOnScreen = cachesOnScreen.Count >= maxCacheCount;

                if (_tooManyCachesOnScreen)
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

        private int _fetchCachesCalls;
        public int FetchCachesCalls
        {
            get { return _fetchCachesCalls; }
            private set
            {
                _fetchCachesCalls = value;
                RaisePropertyChanged(() => FetchCachesCalls);
            }
        }

        private void SetPushpinsOnMap()
        {
            ProcessCaches(null);
            if (!_tooManyCachesOnScreen)
            {
                FetchCachesCalls++;
                apiManager.FetchCaches(ProcessCaches, BoundingRectangle.East, BoundingRectangle.West, BoundingRectangle.North, BoundingRectangle.South);
            }
        }
        
    }
}
