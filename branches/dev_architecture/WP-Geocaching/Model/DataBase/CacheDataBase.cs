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
                if (GetCache(cache.Id) != null) return;
                var newItem = new DbCacheItem
                                  {
                                      Id = cache.Id,
                                      Name = cache.Name,
                                      Latitude = cache.Location.Latitude,
                                      Longitude = cache.Location.Longitude,
                                      UpdateTime = DateTime.Now,
                                      Details = details,
                                      Notebook = notebook
                                  };
                if (cache is GeocachingSuCache)
                {
                    var c = cache as GeocachingSuCache;
                    newItem.Type = (int) c.Type;
                    newItem.Subtype = (int) c.Subtype;
                }
                db.Caches.InsertOnSubmit(newItem);
                db.SubmitChanges();
            }
        }

        public void AddActiveCheckpoint(string cacheId, string name, double latitude, double longitude)
        {
            MakeAllCheckpointsNotActive(cacheId);
            using (var db = new CacheDataContext(ConnectionString))
            {
                int maxId = GetMaxCheckpointIdByCacheId(db.Checkpoints, cacheId);
                var newItem = new DbCheckpointsItem {Id = maxId + 1};

                newItem.Name = name;
                newItem.CacheId = cacheId;
                newItem.Latitude = latitude;
                newItem.Longitude = longitude;
                newItem.Type = (int)GeocachingSuCache.Types.Checkpoint;
                newItem.Subtype = (int)GeocachingSuCache.Subtypes.ActiveCheckpoint;

                db.Checkpoints.InsertOnSubmit(newItem);
                db.SubmitChanges();
            }
        }

        public int GetMaxCheckpointId(string cacheId)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                return GetMaxCheckpointIdByCacheId(db.Checkpoints, cacheId);
            }
        }

        private void MakeAllCheckpointsNotActive(string cacheId)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCacheId(db.Checkpoints, cacheId);
                foreach (DbCheckpointsItem c in query)
                {
                    c.Subtype = (int)GeocachingSuCache.Subtypes.NotActiveCheckpoint;
                }
                db.SubmitChanges();
            }
        }

        public void MakeCheckpointActive(string cacheId, string id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointQuery(db.Checkpoints, cacheId, id);
                DbCheckpointsItem checkpoint = query.FirstOrDefault();
                if ((checkpoint != null) && (checkpoint.Subtype != (int)GeocachingSuCache.Subtypes.ActiveCheckpoint))
                {
                    MakeAllCheckpointsNotActive(checkpoint.CacheId);
                    checkpoint.Subtype = (int)GeocachingSuCache.Subtypes.ActiveCheckpoint;
                }
                db.SubmitChanges();
            }
        }

        public void MakeCacheActive(string cacheId)
        {
            MakeAllCheckpointsNotActive(cacheId);
        }

        public void UpdateCacheInfo(String details, string id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQueryById(db.Caches, id);
                query.FirstOrDefault().Details = details;
                db.SubmitChanges();
            }
        }

        public void UpdateCacheNotebook(String notebook, string id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQueryById(db.Caches, id);
                query.FirstOrDefault().Notebook = notebook;
                db.SubmitChanges();
            }
        }

        public void DeleteCache(string id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQueryById(db.Caches, id);
                var itemForDeleting = query.FirstOrDefault();
                if (itemForDeleting == null) return;
                var settings = new Settings();
                if (settings.LastSoughtCacheId == id)
                {
                    settings.SetDefaultLastSoughtCacheId();
                }
                DeleteAllCheckpoints(id);
                db.Caches.DeleteOnSubmit(itemForDeleting);
                db.SubmitChanges();
            }

            DeletePhotos(id);
        }

        public void DeletePhotos(string cacheId)
        {
            ApiManager.Instance.DeletePhotos(cacheId);
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

        public DbCacheItem GetCache(string id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQueryById(db.Caches, id);
                return query.FirstOrDefault();
            }
        }

        public List<DbCheckpointsItem> GetCheckpointsByCacheId(string cacheId)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCacheId(db.Checkpoints, cacheId);
                return query.ToList();
            }
        }

        public DbCheckpointsItem GetCheckpointByCacheIdAndCheckpointId(string cacheId, int checkpointId)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointQuery(db.Checkpoints, cacheId, checkpointId.ToString());
                return query.FirstOrDefault();
            }
        }

        public void DeleteCheckpoint(string cacheId, string id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointQuery(db.Checkpoints, cacheId, id);
                db.Checkpoints.DeleteOnSubmit(query.FirstOrDefault());
                db.SubmitChanges();
            }
        }

        public void DeleteAllCheckpoints(string cacheId)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCacheId(db.Checkpoints, cacheId);
                db.Checkpoints.DeleteAllOnSubmit(query);
                db.SubmitChanges();
            }
        }

        private IQueryable<DbCacheItem> GetCacheQueryById(Table<DbCacheItem> table, string id)
        {
            var query = from e in table
                        where (e.Id == id)
                        select e;
            return query;
        }

        private IQueryable<DbCheckpointsItem> GetCheckpointQuery(Table<DbCheckpointsItem> table, string cacheId, string id)
        {
            var query = from e in GetCheckpointsQueryByCacheId(table, cacheId)
                        where (e.Id == Convert.ToInt32(id))
                        select e;
            return query;
        }

        private IQueryable<DbCheckpointsItem> GetCheckpointsQueryByCacheId(Table<DbCheckpointsItem> table, string cacheId)
        {
            var query = from e in table
                        where (e.CacheId == cacheId)
                        select e;
            return query;
        }

        private int GetMaxCheckpointIdByCacheId(Table<DbCheckpointsItem> table, string cacheId)
        {
            int maxId = 0;

            var query = (from e in table
                         where (e.CacheId == cacheId)
                         select e.Id);
            if (query.Count() != 0)
            {
                maxId = query.Max();
            }

            return maxId;
        }

        public bool IsContainsCache(string cacheId)
        {
            if (GetCache(cacheId) != null)
            {
                return true;
            }

            return false;
        }
    }
}
