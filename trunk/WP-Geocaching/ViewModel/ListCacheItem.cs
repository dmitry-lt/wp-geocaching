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
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;

namespace WP_Geocaching.ViewModel
{
    public class ListCacheItem
    {        
        private int id;
        private string name;
        private double latitude;
        private double longitude;
        private int type;
        private int subtype;
        private string details;
        private Enum[] iconUri;

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
        public ListCacheItem(DbCacheItem item)
        {
            Id = item.Id;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
            Name = item.Name;
            Subtype = item.Subtype;
            Type = item.Type;
            Details = item.Details;
            IconUri = new Enum[2]{(Cache.Types)item.Type, (Cache.Subtypes)item.Subtype};
        }
        public ListCacheItem(DbCheckpointsItem item)
        {
            Id = item.CheckpointId;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
            Name = item.Name;
            Subtype = item.Subtype;
            Type = item.Type;
            IconUri = new Enum[2] { (Cache.Types)item.Type, (Cache.Subtypes)item.Subtype };
        }
    }
}
