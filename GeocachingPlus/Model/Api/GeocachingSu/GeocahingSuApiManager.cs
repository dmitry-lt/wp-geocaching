using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Globalization;
using GeocachingPlus.Model.Utils;
using System.Text.RegularExpressions;

namespace GeocachingPlus.Model.Api.GeocachingSu
{
    /// <summary>
    /// IApiManager implementation for Geocaching.su
    /// </summary>
    public class GeocahingSuApiManager : IApiManager
    {
        private static readonly Encoding Cp1251Encoding = new CP1251Encoding();
        private const string LinkPattern = "\\s*(?i)href\\s*=\\s*(\"([^\"]*\")|'[^']*'|([^'\">\\s]+))";
        private const string InfoUrl = "http://pda.geocaching.su/cache.php?cid={0}";
        private const string LogbookUrl = "http://pda.geocaching.su/note.php?cid={0}&mode=0";
        private const string PhotosUrl = "http://pda.geocaching.su/pict.php?cid={0}&mode=0";
        private const string DownloadUrl =
            "http://www.geocaching.su/pages/1031.ajax.php?exactly=1&lngmax={0}&lngmin={1}&latmax={2}&latmin={3}&cacheId={4}&geocaching=f1fadbc82d0156ae0f81f7d5e0b26bda";
        private readonly int _id;

        public HashSet<Cache> Caches { get; private set; }

        internal GeocahingSuApiManager()
        {
            var random = new Random();
            _id = random.Next(100000000);
            Caches = new HashSet<Cache>();
        }

        public void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lngmin, double latmax, double latmin)
        {
            var sUrl = String.Format(CultureInfo.InvariantCulture, DownloadUrl, lngmax, lngmin, latmax, latmin, _id);
            var client = new WebClient
                             {
                                 Encoding = Cp1251Encoding
                             };
            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;
                var caches = XElement.Parse(e.Result);
                var parser = new GeocachingSuCacheParser();
                var downloadedCaches = parser.Parse(caches);

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
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        public void FetchCacheDetails(Action<string> processDescription, Action<string> processLogbook, Action<List<string>> processPhotoUrls, Action<string> processHint, Cache cache)
        {
            if (null != processDescription)
            {
                DownloadAndProcessDescription(processDescription, cache);
            }

            if (null != processLogbook)
            {
                DownloadAndProcessLogbook(processLogbook, cache);
            }

            if (null != processPhotoUrls)
            {
                DownloadAndProcessPhotoUrls(processPhotoUrls, cache);
            }

            if (null != processHint)
            {
                processHint("");
            }
        }

        /// <summary>
        /// Downloads data at the url by cacheId
        /// </summary>
        /// <param name="processData">processes downloaded result</param>
        private static void DownloadAndProcessData(string url, Action<string> processData, string cacheId)
        {
            var webClient = new WebClient();

            webClient.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null && processData != null)
                {
                    processData(e.Result);
                }
            };

            webClient.AllowReadStreamBuffering = true;
            webClient.Encoding = Cp1251Encoding;
            webClient.DownloadStringAsync(new Uri(String.Format(url, cacheId), UriKind.Absolute));
        }

        /// <summary>
        /// Downloads cache info
        /// </summary>
        /// <param name="processCacheInfo">processes downloaded result</param>
        /// <param name="cache"> </param>
        private void DownloadAndProcessDescription(Action<string> processCacheInfo, Cache cache)
        {
            DownloadAndProcessData(InfoUrl, processCacheInfo, cache.Id);
        }

        /// <summary>
        /// Downloads cache logbook
        /// </summary>
        /// <param name="processCacheLogbook"> </param>
        /// <param name="cache"> </param>
        private void DownloadAndProcessLogbook(Action<string> processCacheLogbook, Cache cache)
        {
            DownloadAndProcessData(LogbookUrl, processCacheLogbook, cache.Id);
        }

        private void DownloadAndProcessPhotoUrls(Action<List<string>> processPhotoUrls, Cache cache)
        {
            if (null == processPhotoUrls)
            {
                return;
            }

            var webClient = new WebClient();

            webClient.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    return;
                }

                processPhotoUrls(ParsePhotoUrls(e.Result));
            };

            webClient.AllowReadStreamBuffering = true;
            webClient.Encoding = Cp1251Encoding;
            webClient.DownloadStringAsync(new Uri(String.Format(PhotosUrl, cache.Id), UriKind.Absolute));
        }

        private List<string> ParsePhotoUrls(string html)
        {
            var photoUrls = new List<string>();
            var urls = Regex.Matches(html, LinkPattern);

            for (var i = 0; i < urls.Count; i++)
            {
                var url = urls[i].Value.Substring(7, urls[i].Value.Length - 8);

                if (url.EndsWith(".jpg"))
                {
                    photoUrls.Add(url);
                }
            }

            return photoUrls;
        }

    }
}
