using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Device.Location;
using System.ComponentModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.ViewModel
{
    public class CachePushpin
    {
        private GeoCoordinate location;
        private string cacheId;
        private Enum[] iconUri;

        public GeoCoordinate Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }
        public string CacheId
        {
            get
            {
                return this.cacheId;
            }
            set
            {
                this.cacheId = value;
            }
        }
        public Enum[] IconUri
        {
            get
            {
                return this.iconUri;
            }
            set
            {
                this.iconUri = value;
            }
        }

        public CachePushpin()
        {
        }

        public CachePushpin(Cache cache)
        {
            Location = cache.Location;
            CacheId = cache.Id.ToString();
            IconUri = new Enum[2] { cache.Type, cache.Subtype };
        }

        public CachePushpin(DbCheckpointsItem item)
        {
            Location = new GeoCoordinate(item.Latitude, item.Longitude);
            CacheId = "-1";
            IconUri = new Enum[2] { (Cache.Types)item.Type, (Cache.Subtypes)item.Subtype };
        }
    }
}
