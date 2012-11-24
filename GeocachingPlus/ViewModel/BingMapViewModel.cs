using System.Windows;
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

        public override GeoCoordinate MapCenter
        {
            get
            {
                return settings.LatestChooseLocation;
            }
            set
            {
                settings.LatestChooseLocation = value;
                RaisePropertyChanged(() => MapCenter);
            }
        }

        public override int Zoom 
        {
            get
            {
                return settings.LatestChooseZoom;
            }
            set
            {
                settings.LatestChooseZoom = value;
                RaisePropertyChanged(() => Zoom);
            }
        }

        public RequestCounter RequestCounter { get { return RequestCounter.LiveMap; } }

        public BingMapViewModel(IApiManager apiManager)
        {
            MapMode = settings.MapMode;
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

        private void CheckForMoreCacheDetailsInDb(Cache cache)
        {
            if (cache is GeocachingComCache)
            {
                var gcCache = cache as GeocachingComCache;
                
                // check for type and reliable location
                if (gcCache.Type == GeocachingComCache.Types.UNKNOWN || !gcCache.ReliableLocation)
                {
                    var cacheDataBase = new CacheDataBase();
                    var dbCache = cacheDataBase.GetCache(cache.Id, CacheProvider.GeocachingCom);
                    if (null != dbCache)
                    {
                        gcCache.Type = (GeocachingComCache.Types)dbCache.Type;
                        gcCache.Location = new GeoCoordinate(dbCache.Latitude, dbCache.Longitude);
                        gcCache.ReliableLocation = dbCache.ReliableLocation.HasValue && dbCache.ReliableLocation.Value;
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

        private void SetPushpinsOnMap()
        {
            ProcessCaches(null);
            if (!_tooManyCachesOnScreen)
            {
                apiManager.FetchCaches(ProcessCaches, BoundingRectangle.East, BoundingRectangle.West, BoundingRectangle.North, BoundingRectangle.South);
            }
        }
        
    }
}
