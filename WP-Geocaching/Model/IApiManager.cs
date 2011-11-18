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
        WebClient Client {get; set;}
        Action<List<Cache>> SetPushpinsOnMap { get; set; }
        /// <summary>
        /// ToDo: return CacheList
        /// </summary>
        void GetCacheList(double lngmax, double lgnmin, double latmax, double latmin);
    }
}
