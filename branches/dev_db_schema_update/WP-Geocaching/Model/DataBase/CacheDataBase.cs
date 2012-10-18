using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Phone.Data.Linq;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.DataBase.Migration;

namespace WP_Geocaching.Model.DataBase
{
    public class CacheDataBase
    {
        private const string ConnectionString = "Data Source=isostore:/DataBase.sdf";

        // never change the second version const value
        private const int SECOND_VERSION = 2;

        private const int CURRENT_VERSION = 2;

        public CacheDataBase()
        {
            // TODO: move to the constructor in App.xaml.cs to be called only once at app startup

            using (var db = new CacheDataContext(ConnectionString))
            {
                if (!db.DatabaseExists())
                {
                    db.CreateDatabase();
                    db.SubmitChanges();

                    var dbUpdater = db.CreateDatabaseSchemaUpdater();
                            
                    // Add the new database version.
                    dbUpdater.DatabaseSchemaVersion = CURRENT_VERSION;

                    // Perform the database update in a single transaction.
                    dbUpdater.Execute();
                }
                else
                {
                    var dbUpdater = db.CreateDatabaseSchemaUpdater();

                    if (dbUpdater.DatabaseSchemaVersion < SECOND_VERSION)
                    {
                        try
                        {
                            MigrationTool.MigrateToVersion2();
                        }
                        catch
                        {
                            // Even if migration is unsuccessful, update the version (then user will lose all saved caches)

                            // Add the new database version.
                            dbUpdater.DatabaseSchemaVersion = SECOND_VERSION;

                            // Perform the database update in a single transaction.
                            dbUpdater.Execute();
                        }
                    }

/*
                    if (dbUpdater.DatabaseSchemaVersion < 3)
                    {
                        // Add the new database version.
                        dbUpdater.DatabaseSchemaVersion = 3;

                        // Perform the database update in a single transaction.
                        dbUpdater.Execute();
                    }
*/

                }
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
                var newItem = new DbCheckpoint {Id = maxId + 1};

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
                foreach (DbCheckpoint c in query)
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
                DbCheckpoint checkpoint = query.FirstOrDefault();
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

        public List<DbCache> GetCacheList()
        {
            List<DbCache> cacheList;
            using (var db = new CacheDataContext(ConnectionString))
            {
                cacheList = db.Caches.ToList();
            }
            return cacheList;
        }

        public DbCache GetCache(string id, CacheProvider cacheProvider)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQuery(db.Caches, id, cacheProvider);
                return query.FirstOrDefault();
            }
        }

        public List<DbCheckpoint> GetCheckpointsByCache(Cache cache)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointsQueryByCache(db.Checkpoints, cache);
                return query.ToList();
            }
        }

        public DbCheckpoint GetCheckpointByCacheAndCheckpointId(Cache cache, int checkpointId)
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

        private IQueryable<DbCache> GetCacheQuery(Table<DbCache> table, Cache cache)
        {
            return GetCacheQuery(table, cache.Id, cache.CacheProvider);
        }

        private IQueryable<DbCache> GetCacheQuery(Table<DbCache> table, string cacheId, CacheProvider cacheProvider)
        {
            var query = from e in table
                        where (e.Id == cacheId && e.CacheProvider == cacheProvider)
                        select e;
            return query;
        }

        private IQueryable<DbCheckpoint> GetCheckpointQuery(Table<DbCheckpoint> table, Cache cache, string id)
        {
            var query = from e in GetCheckpointsQueryByCache(table, cache)
                        where (e.Id == Convert.ToInt32(id))
                        select e;
            return query;
        }

        private IQueryable<DbCheckpoint> GetCheckpointsQueryByCache(Table<DbCheckpoint> table, Cache cache)
        {
            var query = from e in table
                        where (e.CacheId == cache.Id && e.CacheProvider == cache.CacheProvider)
                        select e;
            return query;
        }

        private int GetMaxCheckpointIdByCache(Table<DbCheckpoint> table, Cache cache)
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
