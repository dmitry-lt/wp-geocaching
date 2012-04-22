using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace WP_Geocaching.Model.DataBase
{
    public class CacheDataBase
    {
        private const string ConnectionString = "Data Source=isostore:/DataBase.sdf";

        public CacheDataBase()
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {
                if (!db.DatabaseExists())
                {
                    db.CreateDatabase();
                    db.SubmitChanges();
                }
            }
        }

        public void AddCache(Cache cache, string details)
        {
            if (cache != null)
            {
                using (var db = new CacheDataContext(ConnectionString))
                {
                    DbCacheItem newItem = new DbCacheItem()
                    {
                        Id = cache.Id,
                        Name = cache.Name,
                        Latitude = cache.Location.Latitude,
                        Longitude = cache.Location.Longitude,
                        Type = (int)cache.Type,
                        Subtype = (int)cache.Subtype,
                        Details = details
                    };

                    if (!db.Caches.Contains(newItem))
                    {
                        db.Caches.InsertOnSubmit(newItem);
                        db.SubmitChanges();
                    }
                }
            }
        }

        public void AddActiveCheckpoint(int cacheId, string name, double latitude, double longitude)
        {
            MakeAllCheckpointsNotActive(cacheId);
            using (var db = new CacheDataContext(ConnectionString))
            {
                int maxId = GetMaxCheckpointIdByCacheId(db.Checkpoints, cacheId);
                DbCheckpointsItem newItem = new DbCheckpointsItem()
                {
                    Id = maxId + 1,
                    CacheId = cacheId,
                    Latitude = latitude,
                    Longitude = longitude,
                    Name = name,
                    Type = (int)Cache.Types.Checkpoint,
                    Subtype = (int)Cache.Subtypes.ActiveCheckpoint,
                };
                db.Checkpoints.InsertOnSubmit(newItem);
                db.SubmitChanges();
            }
        }

        private void MakeAllCheckpointsNotActive(int cacheId)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCacheId(db.Checkpoints, cacheId);
                foreach (DbCheckpointsItem c in query)
                {
                    c.Subtype = (int)Cache.Subtypes.NotActiveCheckpoint;
                }
                db.SubmitChanges();
            }
        }

        public void MakeCheckpointActive(int cacheId, int id)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {

                var query = GetCheckpointQuery(db.Checkpoints, cacheId, id);
                DbCheckpointsItem checkpoint = query.FirstOrDefault();
                if ((checkpoint != null) && (checkpoint.Subtype != (int)Cache.Subtypes.ActiveCheckpoint))
                {
                    MakeAllCheckpointsNotActive(checkpoint.CacheId);
                    checkpoint.Subtype = (int)Cache.Subtypes.ActiveCheckpoint;
                }
                db.SubmitChanges();
            }
        }

        public void MakeCacheActive(int cacheId)
        {
            MakeAllCheckpointsNotActive(cacheId);
        }

        public void UpdateCacheInfo(String details, int id)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQueryById(db.Caches, id);
                query.FirstOrDefault().Details = details;
                db.SubmitChanges();
            }
        }

        public void DeleteCache(int id)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQueryById(db.Caches, id);
                DbCacheItem itemForDeleting = (DbCacheItem)query.FirstOrDefault();
                if (itemForDeleting != null)
                {
                    Settings settings = new Settings();
                    if (settings.LastSoughtCacheId == id)
                    {
                        settings.SetDefaultLastSoughtCacheId();
                    }
                    db.Caches.DeleteOnSubmit(itemForDeleting);
                    db.SubmitChanges();
                }
            }
        }

        public List<DbCacheItem> GetCacheList()
        {
            var cacheList = new List<DbCacheItem>();
            using (var db = new CacheDataContext(ConnectionString))
            {
                cacheList = db.Caches.ToList();
            }
            return cacheList;
        }

        public DbCacheItem GetCache(int id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQueryById(db.Caches, id);
                return query.FirstOrDefault();
            }
        }

        public List<DbCheckpointsItem> GetCheckpointsbyCacheId(int cacheId)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCacheId(db.Checkpoints, cacheId);
                return query.ToList();
            }
        }

        public void DeleteCheckpoint(int cacheId, int id)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointQuery(db.Checkpoints, cacheId, id);
                db.Checkpoints.DeleteOnSubmit((DbCheckpointsItem)query.FirstOrDefault());
                db.SubmitChanges();
            }
        }

        public void DeleteAllCheckpoints(int cacheId)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCacheId(db.Checkpoints, cacheId);
                db.Checkpoints.DeleteAllOnSubmit(query);
                db.SubmitChanges();
            }
        }

        private IQueryable<DbCacheItem> GetCacheQueryById(Table<DbCacheItem> table, int id)
        {
            var query = from e in table
                        where (e.Id == id)
                        select e;
            return query;
        }

        private IQueryable<DbCheckpointsItem> GetCheckpointQuery(Table<DbCheckpointsItem> table, int cacheId, int id)
        {
            var query = from e in GetCheckpointsQueryByCacheId(table, cacheId)
                        where (e.Id == id)
                        select e;
            return query;
        }

        private IQueryable<DbCheckpointsItem> GetCheckpointsQueryByCacheId(Table<DbCheckpointsItem> table, int cacheId)
        {
            var query = from e in table
                        where (e.CacheId == cacheId)
                        select e;
            return query;
        }

        private int GetMaxCheckpointIdByCacheId(Table<DbCheckpointsItem> table, int cacheId)
        {
            int maxId = -1;

            var query = (from e in table
                         where (e.CacheId == cacheId)
                         select e.Id);
            if (query.Count() != 0)
            {
                maxId = query.Max();
            }

            return maxId;
        }
    }
}
