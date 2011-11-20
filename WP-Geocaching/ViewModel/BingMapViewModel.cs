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
        private ObservableCollection<CachePushpin> cachePushpinList;
        private List<Cache> cacheList;
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
        public ObservableCollection<CachePushpin> CachePushpinList
        {
            get
            {
                return this.cachePushpinList;
            }
            set
            {
                this.cachePushpinList = value;
            }
        }
        public List<Cache> CacheList
        {
            get
            {
                return this.cacheList;
            }
            set
            {
                this.cacheList = value;
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
            this.MapCenter = BingMapManager.DefaulMapCenter;
            this.Zoom = BingMapManager.DefaultZoom;
            this.apiManager = apiManager;
            this.CachePushpinList = new ObservableCollection<CachePushpin>();
            this.CacheList = new List<Cache>();
        }

        void ProcessCacheList(List<Cache> caches)
        {
            this.CacheList.AddRange(caches);
            foreach (Cache p in this.CacheList)
            {
                CachePushpin pushpin = new CachePushpin();
                pushpin.Location = p.Location;
                pushpin.Name = p.Id.ToString();
                this.CachePushpinList.Add(pushpin);
            }
        }

        private void GetPushpins()
        {
            this.apiManager.GetCacheList(ProcessCacheList,  BoundingRectangle.East, 
                BoundingRectangle.West, BoundingRectangle.North, BoundingRectangle.South);
        }
    }
}
