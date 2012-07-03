using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using WP_Geocaching.Model.Utils;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.Model
{
    /// <summary>
    /// IApiManager implementation for Geocaching.su
    /// </summary>
    public class GeocahingSuApiManager : IApiManager
    {
        private static GeocahingSuApiManager instance;
        private static readonly Encoding CP1251Encoding = new CP1251Encoding();
        private const string InfoUrl = "http://pda.geocaching.su/cache.php?cid={0}";
        private const string NotebookUrl = "http://pda.geocaching.su/note.php?cid={0}&mode=0";
        private const string DownloadUrl =
            "http://www.geocaching.su/pages/1031.ajax.php?exactly=1&lngmax={0}&lngmin={1}&latmax={2}&latmin={3}&id={4}&geocaching=f1fadbc82d0156ae0f81f7d5e0b26bda";
        private int id;

        public List<Cache> CacheList { get; set; }

        private GeocahingSuApiManager()
        {
            var random = new Random();
            this.id = random.Next(100000000);
            CacheList = new List<Cache>();
        }

        public static GeocahingSuApiManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GeocahingSuApiManager();
                }
                return instance;
            }
        }

        public void UpdateCacheList(Action<List<Cache>> processCacheList, double lngmax, double lngmin, double latmax, double latmin)
        {
            var sUrl = String.Format(CultureInfo.InvariantCulture, DownloadUrl, lngmax, lngmin, latmax, latmin, this.id);
            var client = new WebClient();
            client.Encoding = CP1251Encoding;
            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    var caches = XElement.Parse(e.Result);
                    var parser = new CacheParser();
                    var downloadedCaches = parser.Parse(caches);

                    foreach (Cache p in downloadedCaches)
                    {
                        if (!CacheList.Contains(p))
                        {
                            CacheList.Add(p);
                        }
                    }

                    if (processCacheList != null)
                    {
                        var list = (from cache in CacheList
                                           where ((cache.Location.Latitude <= latmax) &&
                                                  (cache.Location.Latitude >= latmin) &&
                                                  (cache.Location.Longitude <= lngmax) &&
                                                  (cache.Location.Longitude >= lngmin))
                                           select cache).ToList<Cache>();
                        processCacheList(list);
                    }
                }
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        /// <summary>
        /// Downloads data at the url by cacheId
        /// </summary>
        /// <param name="processData">processes downloaded result</param>
        private void DownloadAndProcessData(string url, Action<string> processData, int cacheId)
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
            webClient.Encoding = CP1251Encoding;
            webClient.DownloadStringAsync(new Uri(String.Format(url, cacheId), UriKind.Absolute));
        }

        /// <summary>
        /// Downloads cache info by cacheId
        /// </summary>
        /// <param name="processCacheInfo">processes downloaded result</param>
        public void DownloadAndProcessInfo(Action<string> processCacheInfo, int cacheId)
        {
            DownloadAndProcessData(InfoUrl, processCacheInfo, cacheId);
        }

        /// <summary>
        /// Downloads cache notebook by cacheId
        /// </summary>
        /// <param name="processCacheInfo">processes downloaded result</param>
        public void DownloadAndProcessNotebook(Action<string> processCacheNotebook, int cacheId)
        {
            DownloadAndProcessData(NotebookUrl, processCacheNotebook, cacheId);
        }

        public Cache GetCacheById(int id)
        {
            foreach (Cache p in CacheList)
            {
                if (p.Id == id)
                {
                    return p;
                }
            }
            var db = new CacheDataBase();
            return new Cache(db.GetCache(id));
        }
    }
}
