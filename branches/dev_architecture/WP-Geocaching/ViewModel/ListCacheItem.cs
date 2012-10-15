using System;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;

namespace WP_Geocaching.ViewModel
{
    public class ListCacheItem
    {
        private string id;
        private CacheProvider cacheProvider;
        private string name;
        private double latitude;
        private double longitude;
        private int type;
        private int subtype;
        private string details;
        private Enum[] iconUri;
        private DateTime updateTime;

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public CacheProvider CacheProvider
        {
            get
            {
                return this.cacheProvider;
            }
            set
            {
                this.cacheProvider = value;
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                this.latitude = value;
            }
        }
        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                this.longitude = value;
            }
        }
        public int Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
        public int Subtype
        {
            get
            {
                return this.subtype;
            }
            set
            {
                this.subtype = value;
            }
        }
        public string Details
        {
            get
            {
                return this.details;
            }
            set
            {
                this.details = value;
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
        public DateTime UpdateTime
        {
            get 
            {
                return this.updateTime;
            }
            set
            {
                this.updateTime = value;
            }
        }

        public ListCacheItem(DbCacheItem item)
        {
            Id = item.Id;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
            Name = item.Name;
            Subtype = item.Subtype;
            Type = item.Type;
            UpdateTime = item.UpdateTime;
            Details = item.Details;
            IconUri = new Enum[2] { (Cache.Types)item.Type, (Cache.Subtypes)item.Subtype };
        }

        public ListCacheItem(DbCheckpointsItem item)
        {
            Id = item.Id.ToString();
            CacheProvider = item.CacheProvider;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
            Name = item.Name;
            Subtype = item.Subtype;
            Type = item.Type;
            IconUri = new Enum[2] { (Cache.Types)item.Type, (Cache.Subtypes)item.Subtype };
        }
    }
}
