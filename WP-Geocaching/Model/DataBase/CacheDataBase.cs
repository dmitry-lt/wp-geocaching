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
            using (CacheContext db = new CacheContext(ConnectionString))
            {
                if (!db.DatabaseExists())
                {
                    db.CreateDatabase();                   
                    db.SubmitChanges();
                }
            }
        }

        public void AddNewItem(Cache cache, string details)
        {
            using (var db = new CacheContext(ConnectionString))
            {
                CacheClass newItem = new CacheClass()
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

        public void AddDetailsInItem(String details, int Id)
        {
            using (CacheContext db = new CacheContext(ConnectionString))
            {
                foreach (CacheClass p in db.Caches)
                {
                    if ((p.Id == Id)&&(p.Details == null))
                    {                       
                        p.Details = details;
                        db.SubmitChanges();
						break;
                    }
                }
            }
        }

        public List<CacheClass> GetCacheList()
        {
            var cacheList = new List<CacheClass>();
            using (var db = new CacheContext(ConnectionString))
            {
                var query = from e in db.Caches
                            select e;
                cacheList = query.ToList();
            }
            return cacheList;
        }

        public CacheClass GetItem(int id)
        {
            var cacheList = new List<CacheClass>();
            using (var db = new CacheContext(ConnectionString))
            {
                var query = from e in db.Caches where (e.Id == id)
                            select e;
                cacheList = query.ToList();
            }
            if (cacheList.Count == 0)
            {
                return null;
            }
            return cacheList[0];
        }
    }
}
