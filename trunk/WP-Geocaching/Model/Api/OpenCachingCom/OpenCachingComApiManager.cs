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
        private const string ApiRoot = "http://www.opencaching.com/api/geocache";

        private const string CachesUrl = ApiRoot + "?bbox={0}%2C{1}%2C{2}%2C{3}&type=Traditional+Cache%2CMulti-cache%2CUnknown+Cache%2CVirtual+Cache";

        private const string CacheDescriptionUrl = ApiRoot + "/{0}?description=html";

        private const string PhotoSourceUrl = ApiRoot + "/{0}/{1}";

        private const string CacheDescriptionTemplate = @"<html>
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
</head>
<body>
{0}
</body></html>";

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

        // International UTF-8 Characters in Windows Phone 7 WebBrowser Control
        // See http://matthiasshapiro.com/2010/10/25/international-utf-8-characters-in-windows-phone-7-webbrowser-control/
        private static string ConvertExtendedASCII(string html)
        {
            var s = html.ToCharArray();

            var sb = new StringBuilder();

            foreach (var c in s)
            {
                var intValue = Convert.ToInt32(c);
                if (intValue > 127)
                    sb.AppendFormat("&#{0};", intValue);
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }

        public void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lngmin, double latmax, double latmin)
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

        public void FetchCacheDetails(Action<string> processDescription, Action<string> processLogbook, Action<List<string>> processPhotoUrls, Cache cache)
        {
            var sUrl = String.Format(CultureInfo.InvariantCulture, CacheDescriptionUrl, cache.Id);

            var client = CreateWebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;

                var jsonResult = e.Result;

                var serializer = new DataContractJsonSerializer(typeof(OpenCachingComApiCache));

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonResult)))
                {
                    var parsedCache = (OpenCachingComApiCache)serializer.ReadObject(ms);

                    // description
                    if (null != processDescription)
                    {
                        var description = parsedCache.name + "<br/><br/>" + parsedCache.description;
                        processDescription(String.Format(CacheDescriptionTemplate, ConvertExtendedASCII(description)));
                    }

                    // logs
                    if (null != processLogbook)
                    {
                        var logbook = "";
                        var logs = parsedCache.logs;
                        if (null != logs)
                        {
                            foreach (var log in logs)
                            {
                                logbook += log.user.name + ":<br/>";
                                logbook += log.comment + "<br/><br/>";
                            }
                        }
                        processLogbook(String.Format(CacheDescriptionTemplate, ConvertExtendedASCII(logbook)));
                    }

                    // photos
                    if (null != processPhotoUrls)
                    {
                        var photoUrls = new List<string>();
                        var images = parsedCache.images;
                        if (null != images && images.Any())
                        {
                            photoUrls.AddRange(images.Select(image => String.Format(PhotoSourceUrl, cache.Id, Uri.EscapeUriString(image.caption))));
                        }
                        processPhotoUrls(photoUrls);
                    }
                }
            };

            client.DownloadStringAsync(new Uri(sUrl));
        }

    }
}
