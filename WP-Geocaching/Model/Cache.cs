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

namespace WP_Geocaching.Model
{
    /// <summary>
    /// Contains information about the Cache
    /// </summary>
    public class Cache : IEquatable<Cache>
    {
        private int id;
        private GeoCoordinate location;
        private string name;
        private int type;
        private int subtype;
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

        public bool Equals(Cache cache)
        {
            return this.Id == cache.Id;
        }
    }
}
