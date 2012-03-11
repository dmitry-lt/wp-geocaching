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
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using WP_Geocaching.Model.Utils;

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
        private const string DownloadUrl =
            "http://www.geocaching.su/pages/1031.ajax.php?exactly=1&lngmax={0}&lngmin={1}&latmax={2}&latmin={3}&id={4}&geocaching=f1fadbc82d0156ae0f81f7d5e0b26bda";
        private int id;
        private List<Cache> cacheList;

        public List<Cache> CacheList
        {
            get
            {
                return this.cacheList;
            }
            set
            {
                this.cacheList = value;
            }
        }

        private GeocahingSuApiManager()
        {
            Random random = new Random();
            this.id = random.Next(100000000);
            this.CacheList = new List<Cache>();
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

        public void GetCacheList(Action<List<Cache>> ProcessCacheList, double lngmax, double lngmin, double latmax, double latmin)
        {

            string sUrl = String.Format(CultureInfo.InvariantCulture, DownloadUrl, lngmax, lngmin, latmax, latmin, this.id);
            WebClient client = new WebClient();
            client.Encoding = CP1251Encoding;
            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    XElement caches = XElement.Parse(e.Result);
                    CacheParser parser = new CacheParser();
                    List<Cache> downloadedCaches = parser.Parse(caches);

                    foreach (Cache p in downloadedCaches)
                    {
                        if (!this.cacheList.Contains(p))
                        {
                            this.cacheList.Add(p);
                        }
                    }

                    if (ProcessCacheList != null)
                    {
                     ProcessCacheList(FilterCacheList(lngmax, lngmin, latmax, latmin));
                    }
                }
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        private List<Cache> FilterCacheList(double lngmax, double lngmin, double latmax, double latmin)
        {
            List<Cache> filteredListCache = new List<Cache>();
            foreach (Cache p in this.cacheList)
            {            
                if (((p.Location.Latitude <= latmax) && (p.Location.Latitude  >= latmin) &&
                    (p.Location.Longitude <= lngmax) && (p.Location.Longitude >= lngmin)))
                {
                    filteredListCache.Add(p);
                }
            }
            return filteredListCache;
        }
        public void GetCacheInfo(Action<string> ProcessCacheInfo, int cacheId)
        {
            WebClient webClient = new WebClient();

            webClient.DownloadStringCompleted += (sender, e) =>
                {
                    if (e.Error == null && ProcessCacheInfo != null)
                    {
                        ProcessCacheInfo(e.Result);
                    }
                };

            webClient.AllowReadStreamBuffering = true;
            webClient.Encoding = CP1251Encoding;
            webClient.DownloadStringAsync(new Uri(String.Format(InfoUrl, cacheId), UriKind.Absolute));
        }

        public Cache GetCachebyId(int id)
        {
            foreach (Cache p in GeocahingSuApiManager.Instance.CacheList)
            {
                if (p.Id == id)
                {
                    return p;
                }
            }
            return null;
        }
    }
}
