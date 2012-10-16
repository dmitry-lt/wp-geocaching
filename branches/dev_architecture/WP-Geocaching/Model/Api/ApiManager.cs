using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.Api.OpenCachingCom;

namespace WP_Geocaching.Model.Api
{
    public class ApiManager : IApiManager
    {
        #region geocaching.su

        private readonly GeocahingSuApiManager _geocahingSuApiManager = new GeocahingSuApiManager();

        public void LoadPhotos(string cacheId, Action<ObservableCollection<Photo>> processAction)
        {
            _geocahingSuApiManager.LoadPhotos(cacheId, processAction);
        }

        public void SavePhotos(string cacheId, Action<ObservableCollection<Photo>> processAction)
        {
            _geocahingSuApiManager.SavePhotos(cacheId, processAction);
        }
                
        public void ProcessPhoto(Action<Photo, int> processAction, int index)
        {
            _geocahingSuApiManager.ProcessPhoto(processAction, index);
        }

        public void DeletePhotos(string cacheId)
        {
            _geocahingSuApiManager.DeletePhotos(cacheId);
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
            return _managers[cacheProvider].GetCache(cacheId, cacheProvider);
        }

        public void UpdateCaches(Action<List<Cache>> processCaches, double lngmax, double lgnmin, double latmax, double latmin)
        {
            foreach (var apiManager in _managers.Values)
            {
                apiManager.UpdateCaches(processCaches, lngmax, lgnmin, latmax, latmin);
            }
        }

        public void DownloadAndProcessInfo(Action<string> processCacheInfo, Cache cache)
        {
            _managers[cache.CacheProvider].DownloadAndProcessInfo(processCacheInfo, cache);
        }

        public void DownloadAndProcessNotebook(Action<string> processCacheNotebook, Cache cache)
        {
            _managers[cache.CacheProvider].DownloadAndProcessNotebook(processCacheNotebook, cache);
        }

        #endregion

    }
}
