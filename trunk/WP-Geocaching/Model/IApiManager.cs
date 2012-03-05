using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WP_Geocaching.Model
{
    /// <summary>
    /// Interacts with the external API
    /// </summary>
    public interface IApiManager
    {
        List<Cache> CacheList { get; set; }
        Cache GetCachebyId(int id);
        void GetCacheList(Action<List<Cache>> ProcessCacheList, double lngmax, double lgnmin, double latmax, double latmin);
    }
}
