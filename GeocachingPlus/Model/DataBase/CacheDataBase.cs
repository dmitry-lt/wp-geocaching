using System;
using System.Data.Linq;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using GeocachingPlus.Model.Api.GeocachingCom;
using Microsoft.Phone.Data.Linq;
using GeocachingPlus.Model.Api;
using GeocachingPlus.Model.Api.GeocachingSu;

namespace GeocachingPlus.Model.DataBase
{
    public class CacheDataBase
    {
        private const string ConnectionString = "Data Source=isostore:/DataBase.sdf";

        private const int CurrentDbVersion = 3;

        public static void UpdateSchema()
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                if (!db.DatabaseExists())
                {
                    db.CreateDatabase();
                    db.SubmitChanges();

                    var dbUpdater = db.CreateDatabaseSchemaUpdater();

                    // Add the new database version.
                    dbUpdater.DatabaseSchemaVersion = CurrentDbVersion;

                    // Perform the database update in a single transaction.
                    dbUpdater.Execute();
                }
                else
                {
                    var dbUpdater = db.CreateDatabaseSchemaUpdater();
                    if (dbUpdater.DatabaseSchemaVersion < 3)
                    {
                        // Add the new database version.
                        dbUpdater.DatabaseSchemaVersion = 3;

                        dbUpdater.AddColumn<DbCache>("ReliableLocation");
                        dbUpdater.AddColumn<DbCache>("Hint");

                        // Perform the database update in a single transaction.
                        dbUpdater.Execute();
                    }
                }
            }
        }

        public void SaveCache(Cache cache, string details, string logbook, string hint)
        {
            if (cache == null)
            {
                return;
            }
            using (var db = new CacheDataContext(ConnectionString))
            {
                var dbCache = DbConvert.ToDbCacheItem(cache, details, logbook, hint);

                var query = GetCacheQuery(db.Caches, cache);
                var existingDbCache = query.FirstOrDefault();

                if (existingDbCache == null)
                {
                    // insert
                    db.Caches.InsertOnSubmit(dbCache);
                }
                else
                {
                    // update
                    existingDbCache.Name = dbCache.Name;
                    existingDbCache.Latitude = dbCache.Latitude;
                    existingDbCache.Longitude = dbCache.Longitude;
                    existingDbCache.Type = dbCache.Type;
                    existingDbCache.Subtype = dbCache.Subtype;
                    existingDbCache.UpdateTime = DateTime.Now;
                    existingDbCache.HtmlDescription = dbCache.HtmlDescription;
                    existingDbCache.HtmlLogbook = dbCache.HtmlLogbook;
                    existingDbCache.ReliableLocation = dbCache.ReliableLocation;
                    existingDbCache.Hint = dbCache.Hint;
                }
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
        }

        public void AddActiveCheckpoint(Cache cache, string name, double latitude, double longitude)
        {
            MakeAllCheckpointsNotActive(cache);
            using (var db = new CacheDataContext(ConnectionString))
            {
                var maxId = GetMaxCheckpointIdByCache(db.Checkpoints, cache);
                var newItem = new DbCheckpoint
                {
                    Id = maxId + 1,
                    Name = name,
                    CacheId = cache.Id,
                    CacheProvider = cache.CacheProvider,
                    Latitude = latitude,
                    Longitude = longitude,
                    Type = (int) GeocachingSuCache.Types.Checkpoint,
                    Subtype = (int) GeocachingSuCache.Subtypes.ActiveCheckpoint
                };

                db.Checkpoints.InsertOnSubmit(newItem);
                db.SubmitChanges();
            }
        }

        public void UpdateCheckpoint(Cache cache, int checkpointId, string name, double latitude, double longitude)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCheckpointQuery(db.Checkpoints, cache, checkpointId + "");
                var checkpoint = query.FirstOrDefault();
                checkpoint.Name = name;
                checkpoint.Latitude = latitude;
                checkpoint.Longitude = longitude;
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
                query.FirstOrDefault().HtmlDescription = details;
                db.SubmitChanges();
            }
        }

        public void UpdateCacheLogbook(String logbook, Cache cache)
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var query = GetCacheQuery(db.Caches, cache);
                query.FirstOrDefault().HtmlLogbook = logbook;
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
                if (settings.LatestSoughtCacheId == cache.Id && settings.LatestSoughtCacheProvider == cache.CacheProvider)
                {
                    settings.SetDefaultLastSoughtCache();
                }
                DeleteAllCheckpoints(cache);
                db.Caches.DeleteOnSubmit(itemForDeleting);
                db.SubmitChanges();
            }

            var helper = new FileStorageHelper();
            helper.DeletePhotos(cache);
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
                var query = GetCheckpointQuery(db.Checkpoints, cache, checkpointId.ToString(CultureInfo.InvariantCulture));
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

        public Dictionary<string, GeocachingComLookupInstance> GetGeocachingComLookupDictionary()
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var lookup = db.Caches
                    .Where(c => c.CacheProvider == CacheProvider.GeocachingCom)
                    .ToDictionary(c => c.Id,
                        c =>
                            new GeocachingComLookupInstance
                                {
                                    Id = c.Id,
                                    Type = (GeocachingComCache.Types) c.Type,
                                    Location = new GeoCoordinate(c.Latitude, c.Longitude),
                                    ReliableLocation = c.ReliableLocation.HasValue && c.ReliableLocation.Value,
                                });
                return lookup;
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
