using System;
using System.Device.Location;
using System.Collections.Generic;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.Model
{
    /// <summary>
    /// Contains information about the Cache
    /// </summary>
    public class Cache
    {
        public enum Subtypes
        {
            Valid = 1,
            NotConfirmed = 2,
            NotValid = 3,
            ActiveCheckpoint = 4,
            NotActiveCheckpoint = 5
        }
        public enum Types
        {
            Traditional = 1,
            StepbyStepTraditional = 2, 
            Virtual = 3, 
            Event = 4, 
            Camera = 5, 
            Extreme = 6, 
            StepbyStepVirtual = 7, 
            Competition = 8,
            Checkpoint = 9
        }

        private string id;
        private CacheProvider cacheProvider;
        private GeoCoordinate location;
        private string name;
        private Types type;
        private Subtypes subtype;
        private List<int> cClass;

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
        public Types Type
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
        public Subtypes Subtype
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
        public List<int> CClass
        {
            get
            {
                return cClass;
            }
            set
            {
                cClass = value;
            }
        }

        public Cache(DbCacheItem item)
        {
            Id = item.Id;
            Location = new GeoCoordinate(item.Latitude, item.Longitude);
            Name = item.Name;
            Subtype = (Subtypes)item.Subtype;
            Type = (Types)item.Type;
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
            return other.id == id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
