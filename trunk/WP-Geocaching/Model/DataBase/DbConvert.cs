using System;
using System.Device.Location;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.Api.OpenCachingCom;

namespace WP_Geocaching.Model.DataBase
{
    public class DbConvert
    {
        private static GeocachingSuCache ToGeocachingSuCache(DbCacheItem item)
        {
            var result =
                new GeocachingSuCache()
                {
                    Id = item.Id,
                    Location = new GeoCoordinate(item.Latitude, item.Longitude),
                    Name = item.Name,
                    Subtype = (GeocachingSuCache.Subtypes)item.Subtype,
                    Type = (GeocachingSuCache.Types)item.Type,
                };
            return result;
        }

        private static OpenCachingComCache ToOpenCachingComCache(DbCacheItem item)
        {
            var result =
                new OpenCachingComCache()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Location = new GeoCoordinate(item.Latitude, item.Longitude),
                        Type = (OpenCachingComCache.Types) item.Type,
                    };
            return result;
        }

        public static Cache ToCache(DbCacheItem item)
        {
            switch (item.CacheProvider)
            {
                case CacheProvider.OpenCachingCom:
                    return ToOpenCachingComCache(item);
                
                default:
                    return ToGeocachingSuCache(item);
            }
        }

        private static void InitGeocachingSuSpecificFields(DbCacheItem result, GeocachingSuCache cache)
        {
            result.Type = (int)cache.Type;
            result.Subtype = (int)cache.Subtype;
        }

        private static void InitOpenCachingComSpecificFields(DbCacheItem result, OpenCachingComCache cache)
        {
            result.Type = (int)cache.Type;
        }

        public static DbCacheItem ToDbCacheItem(Cache cache, string details, string notebook)
        {
            var result =
                new DbCacheItem()
                {
                    CacheProvider = cache.CacheProvider,
                    Id = cache.Id,
                    Name = cache.Name,
                    Latitude = cache.Location.Latitude,
                    Longitude = cache.Location.Longitude,
                    UpdateTime = DateTime.Now,
                };

            switch (cache.CacheProvider)
            {
                case CacheProvider.OpenCachingCom:
                    InitOpenCachingComSpecificFields(result, (OpenCachingComCache)cache);
                    break;

                default:
                    InitGeocachingSuSpecificFields(result, (GeocachingSuCache)cache);
                    break;
            }

            result.Details = details;
            result.Notebook = notebook;

            return result;
        }

        public static DbCacheItem ToDbCacheItem(Cache cache)
        {
            return ToDbCacheItem(cache, null, null);
        }

    }
}
