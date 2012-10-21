using System;
using System.Collections.Generic;
using System.Linq;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Model.Api.OpenCachingCom;

namespace GeocachingPlus.Model.Api
{
    public class ApiManager : IApiManager
    {

        #region singleton

        private static readonly ApiManager _instance = new ApiManager();

        public static ApiManager Instance { get { return _instance; } }

        #endregion

        #region IApiManager

        private readonly Dictionary<CacheProvider, IApiManager> _managers = new Dictionary<CacheProvider, IApiManager>();

        private ApiManager()
        {
            _managers.Add(CacheProvider.GeocachingSu, new GeocahingSuApiManager());
            _managers.Add(CacheProvider.OpenCachingCom, new OpenCachingComApiManager());
        }

        public void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lgnmin, double latmax, double latmin)
        {
            foreach (var cp in _managers.Keys.Where(cp => _settings.IsCacheProviderEnabled(cp)))
            {
                _managers[cp].FetchCaches(processCaches, lngmax, lgnmin, latmax, latmin);
            }
        }

        public void FetchCacheDetails(Action<string> processDescription, Action<string> processLogbook, Action<List<string>> processPhotoUrls, Cache cache)
        {
            _managers[cache.CacheProvider].FetchCacheDetails(processDescription, processLogbook, processPhotoUrls, cache);
        }

        #endregion

        #region Settings

        private readonly Settings _settings = new Settings();

        #endregion
    }
}
