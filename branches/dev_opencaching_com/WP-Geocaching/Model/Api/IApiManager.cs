using System;
using System.Collections.Generic;

namespace WP_Geocaching.Model.Api
{
    /// <summary>
    /// Interacts with the external API
    /// </summary>
    public interface IApiManager
    {
        Cache GetCache(string cacheId, CacheProvider cacheProvider);
        void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lgnmin, double latmax, double latmin);
        void FetchCacheDetails(Action<string> processDescription, Action<string> processLogbook, Action<List<string>> processPhotoUrls, Cache cache);
    }
}
