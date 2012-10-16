using System;
using System.Windows.Input;
using System.Device.Location;
using WP_Geocaching.Model;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.ViewModel
{
    public class CachePushpin
    {
        private GeoCoordinate location;
        private string id;
        private CacheProvider cacheProvider;
        private Enum[] iconUri;
        private ICommand showDetails;

        public GeoCoordinate Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }
        public CacheProvider CacheProvider
        {
            get
            {
                return cacheProvider;
            }
            set
            {
                cacheProvider = value;
            }
        }
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public Enum[] IconUri
        {
            get
            {
                return iconUri;
            }
            set
            {
                iconUri = value;
            }
        }
        public ICommand ShowDetails
        {
            get
            {
                return showDetails;
            }
        }

        public CachePushpin()
        {
            showDetails = new ButtonCommand(DefaultShowDetails);
        }

        public CachePushpin(Cache cache)
        {
            Location = cache.Location;
            Id = cache.Id;
            CacheProvider = cache.CacheProvider;

            // TODO: refactor IconUri
            if (cache is GeocachingSuCache)
            {
                var c = cache as GeocachingSuCache;
                IconUri = new Enum[] { c.Type, c.Subtype };
            }

            showDetails = new ButtonCommand(DefaultShowDetails);
        }

        public CachePushpin(DbCheckpointsItem item)
        {
            Location = new GeoCoordinate(item.Latitude, item.Longitude);
            Id = "-1";
            IconUri = new Enum[] { (GeocachingSuCache.Types)item.Type, (GeocachingSuCache.Subtypes)item.Subtype };
        }

        private void DefaultShowDetails(object p)
        {
            if (p != null && !(bool)p)
            {
                NavigationManager.Instance.NavigateToInfoPivot(id, cacheProvider, false);
            }
            NavigationManager.Instance.NavigateToInfoPivot(id, cacheProvider, true);
        }
    }
}
