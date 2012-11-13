namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class GeocachingComCache : Cache
    {
        public bool ReliableLocation { get; set; }

        public GeocachingComCache()
        {
            CacheProvider = CacheProvider.GeocachingCom;
        }
    }
}
