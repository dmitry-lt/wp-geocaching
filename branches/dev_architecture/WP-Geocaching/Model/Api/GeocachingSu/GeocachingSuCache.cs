using System.Collections.Generic;
using System.Device.Location;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.Model.Api.GeocachingSu
{
    public class GeocachingSuCache : Cache
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

        private GeocachingSuCache.Types type;
        private GeocachingSuCache.Subtypes subtype;
        private List<int> cClass;

        public GeocachingSuCache.Types Type
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
        public GeocachingSuCache.Subtypes Subtype
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

        public GeocachingSuCache()
        {
            
        }

        public GeocachingSuCache(DbCacheItem item)
        {
            Id = item.Id;
            Location = new GeoCoordinate(item.Latitude, item.Longitude);
            Name = item.Name;
            Subtype = (GeocachingSuCache.Subtypes)item.Subtype;
            Type = (GeocachingSuCache.Types)item.Type;
        }

    }
}
