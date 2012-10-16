﻿using System;
using System.Device.Location;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;

namespace WP_Geocaching.ViewModel
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

        public ListCacheItem(DbCacheItem item)
        {
            Cache = DbConvert.ToCache(item);

            Subtype = item.Subtype;
            UpdateTime = item.UpdateTime;
            Details = item.Details;
        }

        public ListCacheItem(DbCheckpointsItem item)
        {
            //TODO: refactor
            Cache =
                new GeocachingSuCache
                    {
                        Id = item.Id + "",
                        Location = new GeoCoordinate(item.Latitude, item.Longitude),
                        Name = item.Name,
                        Type = GeocachingSuCache.Types.Checkpoint
                    };

            Subtype = item.Subtype;
        }
    }
}
