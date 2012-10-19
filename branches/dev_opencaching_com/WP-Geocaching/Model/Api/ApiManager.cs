using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.Api.OpenCachingCom;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.Model.Api
{
    public class ApiManager : IApiManager
    {
        #region geocaching.su

        private readonly GeocahingSuApiManager _geocahingSuApiManager = new GeocahingSuApiManager();

        public void SavePhotos(Cache cache, Action<ObservableCollection<Photo>> processAction)
        {
            _geocahingSuApiManager.SavePhotos(cache, processAction);
        }
                
        public void ProcessPhoto(Action<Photo, int> processAction, int index)
        {
            _geocahingSuApiManager.ProcessPhoto(processAction, index);
        }

        public void DeletePhotos(Cache cache)
        {
            _geocahingSuApiManager.DeletePhotos(cache);
        }

        #endregion

        #region singleton

        private static readonly ApiManager _instance = new ApiManager();

        public static ApiManager Instance { get { return _instance; } }

        #endregion

        #region IApiManager

        private readonly Dictionary<CacheProvider, IApiManager> _managers = new Dictionary<CacheProvider, IApiManager>();

        private ApiManager()
        {
            _managers.Add(CacheProvider.GeocachingSu, _geocahingSuApiManager);
            _managers.Add(CacheProvider.OpenCachingCom, new OpenCachingComApiManager());
        }

        public Cache GetCache(string cacheId, CacheProvider cacheProvider)
        {
            var cache = _managers[cacheProvider].GetCache(cacheId, cacheProvider);

            if (null == cache)
            {
                var db = new CacheDataBase();
                cache = DbConvert.ToCache(db.GetCache(cacheId, cacheProvider));
            }

            return cache;

        }

        public void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lgnmin, double latmax, double latmin)
        {
            foreach (var apiManager in _managers.Values)
            {
                apiManager.FetchCaches(processCaches, lngmax, lgnmin, latmax, latmin);
            }
        }

        public void FetchCacheDetails(Action<string> processDescription, Action<string> processLogbook, Action<List<string>> processPhotoUrls, Cache cache)
        {
            _managers[cache.CacheProvider].FetchCacheDetails(processDescription, processLogbook, processPhotoUrls, cache);
        }

        #endregion

    }
}
