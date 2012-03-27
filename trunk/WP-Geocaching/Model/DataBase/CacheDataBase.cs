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
                DbCheckpointsItem newItem = new DbCheckpointsItem()
                {
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

                var query = from e in db.Checkpoints
                            where (e.CacheId == cacheId)
                                select e;

                foreach (DbCheckpointsItem c in query)
                {
                    c.Subtype = (int)Cache.Subtypes.NotActiveCheckpoint;
                }
                db.SubmitChanges();
            }
        }

        public void MakeCheckpointActive(int checkpointId)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {

                var query = from e in db.Checkpoints
                            where (e.CheckpointId == checkpointId)
                            select e;

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
                var query = from e in db.Caches
                            where (e.Id == id)
                            select e;
                query.FirstOrDefault().Details = details;
                db.SubmitChanges();
            }
        }

        public void DeleteCache(int id)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {
                var query = from e in db.Caches
                            where (e.Id == id)
                            select e;
                db.Caches.DeleteOnSubmit((DbCacheItem)query.FirstOrDefault());
                db.SubmitChanges();
            }
        }

        public List<DbCacheItem> GetCacheList()
        {
            var cacheList = new List<DbCacheItem>();
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = from e in db.Caches
                            select e;
                cacheList = query.ToList();
            }
            return cacheList;
        }

        public DbCacheItem GetCache(int id)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = from e in db.Caches
                            where (e.Id == id)
                            select e;
                return query.FirstOrDefault();
            }
        }

        public List<DbCheckpointsItem> GetCheckpointsbyCacheId(int cacheId)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = from e in db.Checkpoints
                            where (e.CacheId == cacheId)
                            select e;
                return query.ToList();
            }
        }

        public void DeleteCheckpoint(int id)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {
                var query = from e in db.Checkpoints
                            where (e.CheckpointId == id)
                            select e;
                db.Checkpoints.DeleteOnSubmit((DbCheckpointsItem)query.FirstOrDefault());
                db.SubmitChanges();
            }
        }

        public void DeleteAllCheckpoints(int cacheId)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {

                var query = from e in db.Checkpoints
                            where (e.CacheId == cacheId)
                            select e;

                db.Checkpoints.DeleteAllOnSubmit(query);
                db.SubmitChanges();
            }
        }
    }
}
