namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class GCConstants
    {
        public const string GC_URL = "http://www.geocaching.com/";
        /** Live Map */
        public const string URL_LIVE_MAP = GC_URL + "map/default.aspx";
        /** Live Map pop-up */
        public const string URL_LIVE_MAP_DETAILS = GC_URL + "map/map.details";
        /** Caches in a tile */
        public const string URL_MAP_INFO = GC_URL + "map/map.info";
        /** Tile itself */
        public const string URL_MAP_TILE = GC_URL + "map/map.png";
        // Info box top-right
        public const string PATTERN_LOGIN_NAME = "\"SignedInProfileLink\">([^<]+)</a>";
        public const string PATTERN_LOGIN_NAME_LOGIN_PAGE = "<h4>Success:</h4> <p>You are logged in as[^<]*<strong><span id=\"ctl00_ContentBody_lbUsername\"[^>]*>([^<]+)[^<]*</span>";
    }
}
