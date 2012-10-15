using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WP_Geocaching.Model.Api
{
    public class ApiManager : IApiManager
    {
        #region geocaching.su

        private readonly GeocahingSuApiManager _geocahingSuApiManager = new GeocahingSuApiManager();

        public void LoadPhotos(int cacheId, Action<ObservableCollection<Photo>> processAction)
        {
            _geocahingSuApiManager.LoadPhotos(cacheId, processAction);
        }

        public void SavePhotos(int cacheId, Action<ObservableCollection<Photo>> processAction)
        {
            _geocahingSuApiManager.SavePhotos(cacheId, processAction);
        }
                
        public void ProcessPhoto(Action<Photo, int> processAction, int index)
        {
            _geocahingSuApiManager.ProcessPhoto(processAction, index);
        }

        public void DownloadAndProcessNotebook(Action<string> processCacheNotebook, int cacheId)
        {
            _geocahingSuApiManager.DownloadAndProcessNotebook(processCacheNotebook, cacheId);
        }

        public void DownloadAndProcessInfo(Action<string> processCacheInfo, int cacheId)
        {
            _geocahingSuApiManager.DownloadAndProcessInfo(processCacheInfo, cacheId);
        }

        public void DeletePhotos(int cacheId)
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

        public Cache GetCacheById(int cacheId)
        {
            return _geocahingSuApiManager.GetCacheById(cacheId);
        }

        public void UpdateCaches(Action<List<Cache>> processCaches, double lngmax, double lgnmin, double latmax, double latmin)
        {
            _geocahingSuApiManager.UpdateCaches(processCaches, lngmax, lgnmin, latmax, latmin);
        }

        #endregion

    }
}
