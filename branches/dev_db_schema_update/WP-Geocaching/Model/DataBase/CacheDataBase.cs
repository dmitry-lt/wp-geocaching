using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Api.GeocachingSu;

namespace WP_Geocaching.Model.DataBase
{
    public class CacheDataBase
    {
        private const string ConnectionString = "Data Source=isostore:/DataBase.sdf";

        public CacheDataBase()
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                if (db.DatabaseExists())
                {
                    return;
                }
                db.CreateDatabase();
                db.SubmitChanges();
            }
        }

        public void AddCache(Cache cache, string details, string notebook)
        {
            if (cache == null)
            {
                return;
            }
            using (var db = new CacheDataContext(ConnectionString))
            {
                if (GetCache(cache.Id, cache.CacheProvider) != null) return;

                var newItem = DbConvert.ToDbCacheItem(cache, details, notebook);

                db.Caches.InsertOnSubmit(newItem);
                db.SubmitChanges();
            }
        }

        public void AddActiveCheckpoint(Cache cache, string name, double latitude, double longitude)
        {
            MakeAllCheckpointsNotActive(cache);
            using (var db = new CacheDataContext(ConnectionString))
            {
                int maxId = GetMaxCheckpointIdByCache(db.Checkpoints, cache);
                var newItem = new DbCheckpointsItem {Id = maxId + 1};

                newItem.Name = name;
                newItem.CacheId = cache.Id;
                newItem.CacheProvider = cache.CacheProvider;
                newItem.Latitude = latitude;
                newItem.Longitude = longitude;
                newItem.Type = (int)GeocachingSuCache.Types.Checkpoint;
                newItem.Subtype = (int)GeocachingSuCache.Subtypes.ActiveCheckpoint;

                db.Checkpoints.InsertOnSubmit(newItem);
                db.SubmitChanges();
            }
        }

        public int GetMaxCheckpointId(Cache cache)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                return GetMaxCheckpointIdByCache(db.Checkpoints, cache);
            }
        }

        private void MakeAllCheckpointsNotActive(Cache cache)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCache(db.Checkpoints, cache);
                foreach (DbCheckpointsItem c in query)
                {
                    c.Subtype = (int)GeocachingSuCache.Subtypes.NotActiveCheckpoint;
                }
                db.SubmitChanges();
            }
        }

        public void MakeCheckpointActive(Cache cache, string id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointQuery(db.Checkpoints, cache, id);
                DbCheckpointsItem checkpoint = query.FirstOrDefault();
                if ((checkpoint != null) && (checkpoint.Subtype != (int)GeocachingSuCache.Subtypes.ActiveCheckpoint))
                {
                    MakeAllCheckpointsNotActive(cache);
                    checkpoint.Subtype = (int)GeocachingSuCache.Subtypes.ActiveCheckpoint;
                }
                db.SubmitChanges();
            }
        }

        public void MakeCacheActive(Cache cache)
        {
            MakeAllCheckpointsNotActive(cache);
        }

        public void UpdateCacheInfo(String details, Cache cache)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQuery(db.Caches, cache);
                query.FirstOrDefault().Details = details;
                db.SubmitChanges();
            }
        }

        public void UpdateCacheNotebook(String notebook, Cache cache)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQuery(db.Caches, cache);
                query.FirstOrDefault().Notebook = notebook;
                db.SubmitChanges();
            }
        }

        public void DeleteCache(Cache cache)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQuery(db.Caches, cache);
                var itemForDeleting = query.FirstOrDefault();
                if (itemForDeleting == null) return;
                var settings = new Settings();
                if (settings.LastSoughtCacheId == cache.Id)
                {
                    settings.SetDefaultLastSoughtCacheId();
                }
                DeleteAllCheckpoints(cache);
                db.Caches.DeleteOnSubmit(itemForDeleting);
                db.SubmitChanges();
            }

            ApiManager.Instance.DeletePhotos(cache);
        }

        public List<DbCacheItem> GetCacheList()
        {
            List<DbCacheItem> cacheList;
            using (var db = new CacheDataContext(ConnectionString))
            {
                cacheList = db.Caches.ToList();
            }
            return cacheList;
        }

        public DbCacheItem GetCache(string id, CacheProvider cacheProvider)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQuery(db.Caches, id, cacheProvider);
                return query.FirstOrDefault();
            }
        }

        public List<DbCheckpointsItem> GetCheckpointsByCache(Cache cache)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCache(db.Checkpoints, cache);
                return query.ToList();
            }
        }

        public DbCheckpointsItem GetCheckpointByCacheAndCheckpointId(Cache cache, int checkpointId)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointQuery(db.Checkpoints, cache, checkpointId.ToString());
                return query.FirstOrDefault();
            }
        }

        public void DeleteCheckpoint(Cache cache, string id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointQuery(db.Checkpoints, cache, id);
                db.Checkpoints.DeleteOnSubmit(query.FirstOrDefault());
                db.SubmitChanges();
            }
        }

        public void DeleteAllCheckpoints(Cache cache)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCache(db.Checkpoints, cache);
                db.Checkpoints.DeleteAllOnSubmit(query);
                db.SubmitChanges();
            }
        }

        private IQueryable<DbCacheItem> GetCacheQuery(Table<DbCacheItem> table, Cache cache)
        {
            return GetCacheQuery(table, cache.Id, cache.CacheProvider);
        }

        private IQueryable<DbCacheItem> GetCacheQuery(Table<DbCacheItem> table, string cacheId, CacheProvider cacheProvider)
        {
            var query = from e in table
                        where (e.Id == cacheId && e.CacheProvider == cacheProvider)
                        select e;
            return query;
        }

        private IQueryable<DbCheckpointsItem> GetCheckpointQuery(Table<DbCheckpointsItem> table, Cache cache, string id)
        {
            var query = from e in GetCheckpointsQueryByCache(table, cache)
                        where (e.Id == Convert.ToInt32(id))
                        select e;
            return query;
        }

        private IQueryable<DbCheckpointsItem> GetCheckpointsQueryByCache(Table<DbCheckpointsItem> table, Cache cache)
        {
            var query = from e in table
                        where (e.CacheId == cache.Id && e.CacheProvider == cache.CacheProvider)
                        select e;
            return query;
        }

        private int GetMaxCheckpointIdByCache(Table<DbCheckpointsItem> table, Cache cache)
        {
            int maxId = 0;

            var query = (from e in table
                         where (e.CacheId == cache.Id && e.CacheProvider == cache.CacheProvider)
                         select e.Id);
            if (query.Count() != 0)
            {
                maxId = query.Max();
            }

            return maxId;
        }

    }
}
