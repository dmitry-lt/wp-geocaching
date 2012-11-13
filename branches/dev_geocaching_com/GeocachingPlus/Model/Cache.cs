using System;
using System.Device.Location;
using GeocachingPlus.Model.Api;

namespace GeocachingPlus.Model
{
    /// <summary>
    /// Contains information about the Cache
    /// </summary>
    public class Cache
    {
        public string Id { get; set; }
        public CacheProvider CacheProvider { get; set; }
        public GeoCoordinate Location { get; set; }
        public string Name { get; set; }

        public override bool Equals(Object cache)
        {
            if (ReferenceEquals(null, cache)) return false;
            if (ReferenceEquals(this, cache)) return true;
            if (!(cache is Cache)) return false;
            return Equals((Cache)cache);
        }

        public bool Equals(Cache other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id && other.CacheProvider == CacheProvider;
        }

        public override int GetHashCode()
        {
            return (CacheProvider.GetHashCode() * 17) ^ Id.GetHashCode();
        }
    }
}
