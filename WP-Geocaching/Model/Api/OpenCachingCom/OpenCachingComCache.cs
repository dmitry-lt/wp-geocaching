namespace WP_Geocaching.Model.Api.OpenCachingCom
{
    public class OpenCachingComCache : Cache
    {
        public enum Types
        {
            Traditional = 1,
            Multi = 2,
            Puzzle = 3,
            Virtual = 4,

            Checkpoint = 9
        }

        public OpenCachingComCache()
        {
            CacheProvider = CacheProvider.OpenCachingCom;
        }

        public Types Type { get; set; }

    }
}
