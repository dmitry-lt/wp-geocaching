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
using System.Collections.Generic;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.Model
{
    /// <summary>
    /// Contains information about the Cache
    /// </summary>
    public class Cache : IEquatable<Cache>
    {
        public enum Subtypes: int
        {
            Valid = 1,
            NotConfirmed = 2,
            NotValid = 3,
            ActiveCheckpoint = 4,
            NotActiveCheckpoint = 5
        }
        public enum Types : int
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

        private int id;
        private GeoCoordinate location;
        private string name;
        private Types type;
        private Subtypes subtype;
        private List<int> cClass;

        public int Id
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
                return this.cClass;
            }
            set
            {
                this.cClass = value;
            }
        }

        public Cache(DbCacheItem item)
        {
            this.Id = item.Id;
            this.Location = new GeoCoordinate(item.Latitude, item.Longitude);
            this.Name = item.Name;
            this.Subtype = (Subtypes)item.Subtype;
            this.Type = (Types)item.Type;
        }

        public Cache() { }

        public bool Equals(Cache cache)
        {
            return this.Id == cache.Id;
        }
    }
}
