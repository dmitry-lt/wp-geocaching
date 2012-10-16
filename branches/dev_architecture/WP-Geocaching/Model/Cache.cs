using System;
using System.Device.Location;
using System.Collections.Generic;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.Model
{
    /// <summary>
    /// Contains information about the Cache
    /// </summary>
    public class Cache
    {
        private string id;
        private CacheProvider cacheProvider;
        private GeoCoordinate location;
        private string name;

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
            protected set
            {
                this.cacheProvider = value;
            }
        }
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

        public Cache() { }

        public override bool Equals(Object cache)
        {
            if (ReferenceEquals(null, cache)) return false;
            if (ReferenceEquals(this, cache)) return true;
            if (cache.GetType() != typeof(Cache)) return false;
            return Equals((Cache)cache);
        }


        public bool Equals(Cache other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.id == id && other.CacheProvider == CacheProvider;
        }

        public override int GetHashCode()
        {
            return (CacheProvider.GetHashCode() * 17) ^ id.GetHashCode();
        }
    }
}
