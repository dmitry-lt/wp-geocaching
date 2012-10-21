using System;
using System.Device.Location;
using GeocachingPlus.Model.Api;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.Model;

namespace GeocachingPlus.ViewModel
{
    public class ListCacheItem
    {
        private int subtype;
        private string details;
        private DateTime updateTime;

        public Cache Cache { get; private set; }

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

        public ListCacheItem(DbCache item)
        {
            Cache = DbConvert.ToCache(item);

            Subtype = item.Subtype;
            UpdateTime = item.UpdateTime;
            Details = item.HtmlDescription;
        }

        public ListCacheItem(DbCheckpoint item)
        {
            //TODO: refactor
            Cache =
                new GeocachingSuCache
                    {
                        Id = item.Id + "",
                        Location = new GeoCoordinate(item.Latitude, item.Longitude),
                        Name = item.Name,
                        Type = GeocachingSuCache.Types.Checkpoint,
                        Subtype = (GeocachingSuCache.Subtypes)item.Subtype,
                    };

            Subtype = item.Subtype;
        }
    }
}
