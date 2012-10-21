namespace GeocachingPlus.Model.Api.OpenCachingCom
{
    public class OpenCachingComCache : Cache
    {
        public enum Types
        {
            Traditional = 101,
            Multi = 102,
            Puzzle = 103,
            Virtual = 104,
        }

        public OpenCachingComCache()
        {
            CacheProvider = CacheProvider.OpenCachingCom;
        }

        public Types Type { get; set; }

    }
}
