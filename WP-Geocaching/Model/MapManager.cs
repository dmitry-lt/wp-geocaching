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

namespace WP_Geocaching.Model
{
    /// <summary>
    /// Contains information for BingMap
    /// </summary>
    public class MapManager
    {
        private static MapManager instance;
        private GeoCoordinate defaultMapCenter = new GeoCoordinate(59.879904, 29.828674);
        private int cacheId;
        private int defaultZoom = 13;

        public static MapManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapManager();
                }
                return instance;
            }
        }

        public GeoCoordinate DefaulMapCenter
        {
            get
            {
                return this.defaultMapCenter;
            }
        }
        public int CacheId
        {
            get
            {
                return this.cacheId;
            }
            set
            {
                this.cacheId = value;
            }
        }
        public int DefaultZoom
        {
            get
            {
                return this.defaultZoom;
            }
        }
        private MapManager() { }
    }
}
