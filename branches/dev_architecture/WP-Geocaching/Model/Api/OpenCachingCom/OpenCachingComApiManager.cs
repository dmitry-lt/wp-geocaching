using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WP_Geocaching.Model.Api.OpenCachingCom
{
    public class OpenCachingComApiManager : IApiManager
    {
        private const string CachesUrl =
            "http://www.opencaching.com/api/geocache?bbox={0}%2C{1}%2C{2}%2C{3}&type=Traditional+Cache%2CMulti-cache%2CUnknown+Cache%2CVirtual+Cache";

        private const string CacheDescriptionUrl = "http://www.opencaching.com/api/geocache/{0}?description=html";

        internal OpenCachingComApiManager()
        {
            Caches = new HashSet<Cache>();
        }

        public HashSet<Cache> Caches { get; private set; }

        private WebClient CreateWebClient()
        {
            var client = new WebClient();
            client.Headers["Authorization"] = "jammerwww";
            return client;
        }

        private OpenCachingComCache.Types ParseType(string type)
        {
            switch (type)
            {
                case "Multi-cache":
                    return OpenCachingComCache.Types.Multi;

                case "Unknown Cache":
                    return OpenCachingComCache.Types.Puzzle;

                case "Virtual Cache":
                    return OpenCachingComCache.Types.Virtual;

                default:
                    return OpenCachingComCache.Types.Traditional;
            }
        }

        public Cache GetCache(string cacheId, CacheProvider cacheProvider)
        {
            return Caches.FirstOrDefault(c => c.Id == cacheId);
        }

        public void UpdateCaches(Action<List<Cache>> processCaches, double lngmax, double lngmin, double latmax, double latmin)
        {
            var sUrl = String.Format(CultureInfo.InvariantCulture, CachesUrl, latmin, lngmin, latmax, lngmax);

            var client = CreateWebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;

                var jsonResult = e.Result;

                var serializer = new DataContractJsonSerializer(typeof(OpenCachingComApiCache[]));

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonResult)))
                {
                    var parsedCaches = (OpenCachingComApiCache[])serializer.ReadObject(ms);

                    var downloadedCaches = new List<Cache>();

                    foreach (var parsedCache in parsedCaches)
                    {
                        downloadedCaches.Add(
                            new OpenCachingComCache()
                            {
                                Id = parsedCache.oxcode,
                                Name = parsedCache.name,
                                Location = new 
                                    GeoCoordinate()
                                               {
                                                   Latitude = Convert.ToDouble(parsedCache.location.lat, CultureInfo.InvariantCulture),
                                                   Longitude = Convert.ToDouble(parsedCache.location.lon, CultureInfo.InvariantCulture),
                                               },
                                Type = ParseType(parsedCache.type),
                            });
                    }

                    foreach (var p in downloadedCaches)
                    {
                        if (!Caches.Contains(p))
                        {
                            Caches.Add(p);
                        }
                    }

                    if (processCaches == null) return;
                    var list = (from cache in Caches
                                where ((cache.Location.Latitude <= latmax) &&
                                       (cache.Location.Latitude >= latmin) &&
                                       (cache.Location.Longitude <= lngmax) &&
                                       (cache.Location.Longitude >= lngmin))
                                select cache).ToList<Cache>();
                    processCaches(list);
                }
            };

            client.DownloadStringAsync(new Uri(sUrl));
        }

        public void DownloadAndProcessInfo(Action<string> processCacheInfo, Cache cache)
        {
            throw new NotImplementedException();
        }
    }
}
