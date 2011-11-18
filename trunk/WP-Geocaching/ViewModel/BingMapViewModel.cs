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

namespace WP_Geocaching.ViewModel
{
    public class BingMapViewModel
    {
        private int zoom;
        public GeoCoordinate mapCenter;
        private IApiManager apiManager;
        private ObservableCollection<CachePushpin> cachePushpinList;      
        private List<Cache> cacheList;
       
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

        public BingMapViewModel(IApiManager apiManager)
        {
            this.apiManager = apiManager;
            this.CachePushpinList = new ObservableCollection<CachePushpin>();
            this.CacheList = new List<Cache>();
            this.apiManager.SetPushpinsOnMap = SetPushpinsOnMap;
        }

        void SetPushpinsOnMap(List<Cache> caches)
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
    }
}
