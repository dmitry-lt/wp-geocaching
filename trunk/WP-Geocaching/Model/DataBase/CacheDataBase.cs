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
        public const string ConnectionString = "Data Source=isostore:/DataBase.sdf";

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
            using (var db = new CacheDataContext(ConnectionString))
            {
                DbCacheItem newItem = new DbCacheItem()
                {
                    Id = cache.Id,
                    Name = cache.Name,
                    Latitude = cache.Location.Latitude,
                    Longitude = cache.Location.Longitude,
                    Type = cache.Type,
                    Subtype = cache.Subtype,
                    Details = details
                };

                if (!db.Caches.Contains(newItem))
                {
                    db.Caches.InsertOnSubmit(newItem);
                    db.SubmitChanges();
                }
            }          
        }

        public void UpdateCacheInfo(String details, int id)
        {
            using (CacheDataContext db = new CacheDataContext(ConnectionString))
            {
                foreach (DbCacheItem p in db.Caches)
                {
                    if ((p.Id == id) && (p.Details == null))
                    {
                        p.Details = details;
                        db.SubmitChanges();
                        break;
                    }
                }
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
    }
}
