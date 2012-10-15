namespace WP_Geocaching.Model
{
    /// <summary>
    /// Contains information for BingMap
    /// </summary>
    public class MapManager
    {
        private static MapManager instance;
        private const int defaultZoom = 13;

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

        public int CacheId { get; set; }
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
