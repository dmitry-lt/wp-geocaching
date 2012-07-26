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
                return defaultMapCenter;
            }
        }
        public int CacheId
        {
            get
            {
                return cacheId;
            }
            set
            {
                cacheId = value;
            }
        }
        public int DefaultZoom
        {
            get
            {
                return defaultZoom;
            }
        }
        private MapManager() { }
    }
}
