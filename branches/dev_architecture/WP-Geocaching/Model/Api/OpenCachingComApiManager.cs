using System;
using System.Collections.Generic;

namespace WP_Geocaching.Model.Api
{
    public class OpenCachingComApiManager : IApiManager
    {
        internal OpenCachingComApiManager()
        {
            Caches = new HashSet<Cache>();
        }

        public HashSet<Cache> Caches { get; private set; }

        public Cache GetCacheById(int cacheId)
        {
            throw new NotImplementedException();
        }

        public void UpdateCaches(Action<List<Cache>> processCaches, double lngmax, double lgnmin, double latmax, double latmin)
        {
            throw new NotImplementedException();
        }
    }
}
