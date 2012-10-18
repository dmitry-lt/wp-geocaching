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

        private Types type;
        private Subtypes subtype;
        private List<int> cClass;

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

        public GeocachingSuCache()
        {
            CacheProvider = CacheProvider.GeocachingSu;
        }

    }
}
