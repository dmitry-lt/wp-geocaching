using System;
using System.Device.Location;
using GeocachingPlus.Model.Api;
using GeocachingPlus.Model.Api.GeocachingCom;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Model.Api.OpenCachingCom;
using GeocachingPlus.Model.Api.OpencachingDe;

namespace GeocachingPlus.Model.DataBase
{
    public class DbConvert
    {
        private static GeocachingSuCache ToGeocachingSuCache(DbCache item)
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

        private static OpenCachingComCache ToOpenCachingComCache(DbCache item)
        {
            var result =
                new OpenCachingComCache()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Location = new GeoCoordinate(item.Latitude, item.Longitude),
                    Type = (OpenCachingComCache.Types)item.Type,
                };
            return result;
        }

        private static GeocachingComCache ToGeocachingComCache(DbCache item)
        {
            var result =
                new GeocachingComCache()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Location = new GeoCoordinate(item.Latitude, item.Longitude),
                    Type = (GeocachingComCache.Types)item.Type,
                    ReliableLocation = item.ReliableLocation.HasValue && item.ReliableLocation.Value,
                };
            return result;
        }

        private static OpencachingDeCache ToOpencachingDeCache(DbCache item)
        {
            var result =
                new OpencachingDeCache()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Location = new GeoCoordinate(item.Latitude, item.Longitude),
                    Type = (OpencachingDeCache.Types)item.Type,
                };
            return result;
        }

        public static Cache ToCache(DbCache item)
        {
            switch (item.CacheProvider)
            {
                case CacheProvider.OpenCachingCom:
                    return ToOpenCachingComCache(item);

                case CacheProvider.GeocachingCom:
                    return ToGeocachingComCache(item);

                case CacheProvider.OpencachingDe:
                    return ToOpencachingDeCache(item);

                default:
                    return ToGeocachingSuCache(item);
            }
        }

        private static void InitGeocachingSuSpecificFields(DbCache result, GeocachingSuCache cache)
        {
            result.Type = (int)cache.Type;
            result.Subtype = (int)cache.Subtype;
        }

        private static void InitOpenCachingComSpecificFields(DbCache result, OpenCachingComCache cache)
        {
            result.Type = (int)cache.Type;
        }

        private static void InitGeocachingComSpecificFields(DbCache result, GeocachingComCache cache)
        {
            result.Type = (int)cache.Type;
            result.ReliableLocation = cache.ReliableLocation;
        }

        private static void InitOpencachingDeSpecificFields(DbCache result, OpencachingDeCache cache)
        {
            result.Type = (int)cache.Type;
        }

        public static DbCache ToDbCacheItem(Cache cache, string details, string logbook, string hint)
        {
            var result =
                new DbCache()
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

                case CacheProvider.GeocachingCom:
                    InitGeocachingComSpecificFields(result, (GeocachingComCache)cache);
                    break;

                case CacheProvider.OpencachingDe:
                    InitOpencachingDeSpecificFields(result, (OpencachingDeCache)cache);
                    break;

                default:
                    InitGeocachingSuSpecificFields(result, (GeocachingSuCache)cache);
                    break;
            }

            result.HtmlDescription = details;
            result.HtmlLogbook = logbook;
            result.Hint = hint;

            return result;
        }

        [Obsolete]
        public static DbCache ToDbCacheItem(Cache cache)
        {
            return ToDbCacheItem(cache, null, null, null);
        }

    }
}
