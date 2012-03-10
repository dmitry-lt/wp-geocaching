﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.Text;
using System.IO;
using System.Xml.Linq;
using WP_Geocaching.Model;
using System.ComponentModel;
using System.Collections.Generic;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;

namespace WP_Geocaching.ViewModel
{
    public class BingMapViewModel
    {
        private int zoom;
        private GeoCoordinate mapCenter;
        private IApiManager apiManager;
        private ObservableCollection<CachePushpin> cachePushpinCollection;
        private LocationRect boundingRectangle;

        public int Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                this.zoom = value;
            }
        }
        public GeoCoordinate MapCenter
        {
            get
            {
                return this.mapCenter;
            }
            set
            {
                this.mapCenter = value;
            }
        }
        public ObservableCollection<CachePushpin> CachePushpinCollection
        {
            get
            {
                return this.cachePushpinCollection;
            }
            set
            {
                this.cachePushpinCollection = value;
            }
        }
        
        public LocationRect BoundingRectangle
        {
            get
            {
                return this.boundingRectangle;
            }
            set
            {
                this.boundingRectangle = value;
                this.GetPushpins();
            }
        }

        public BingMapViewModel(IApiManager apiManager)
        {
            BingMapManager manager = new BingMapManager();
            this.MapCenter = manager.DefaulMapCenter;
            this.Zoom = manager.DefaultZoom;
            this.apiManager = apiManager;
            this.CachePushpinCollection = new ObservableCollection<CachePushpin>();
        }

        private void ProcessCacheList(List<Cache> caches)
        {
            foreach (Cache p in caches)
            {
                if (!this.apiManager.CacheList.Contains(p))
                {
                    this.apiManager.CacheList.Add(p);
                }
            }
            this.CachePushpinCollection.Clear();
            foreach (Cache p in this.apiManager.CacheList)
            {
                if ((p.Location.Latitude <= BoundingRectangle.North) &&
                    (p.Location.Latitude >= BoundingRectangle.South) &&
                    (p.Location.Longitude <= BoundingRectangle.East) &&
                    (p.Location.Longitude >= BoundingRectangle.West))
                {
                    CachePushpin pushpin = new CachePushpin()
                    {
                        Location = p.Location,
                        CacheId = p.Id.ToString(),
                        IconUri = new Uri(p.Type.ToString() + p.Subtype.ToString(), UriKind.Relative),
                    };
                    this.CachePushpinCollection.Add(pushpin);
                }
            }
        }

        private void GetPushpins()
        {
            this.apiManager.GetCacheList(ProcessCacheList,  BoundingRectangle.East, 
                BoundingRectangle.West, BoundingRectangle.North, BoundingRectangle.South);
        }
    }
}
