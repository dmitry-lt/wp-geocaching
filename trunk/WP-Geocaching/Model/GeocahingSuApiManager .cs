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
            string sUrl = "http://www.geocaching.su/pages/1031.ajax.php?exactly=1&lngmax=" +
                Convert.ToString(lngmax, CultureInfo.InvariantCulture) + "&lngmin="
                + Convert.ToString(lngmin, CultureInfo.InvariantCulture) + "&latmax="
                + Convert.ToString(latmax, CultureInfo.InvariantCulture) + "&latmin="
                + Convert.ToString(latmin, CultureInfo.InvariantCulture) + "&id=" + this.id
                + "&geocaching=f1fadbc82d0156ae0f81f7d5e0b26bda";
            WebClient client = new WebClient();
            client.Encoding = CP1251Encoding;
            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    XElement caches = XElement.Parse(e.Result);
                    CacheParser parser = new CacheParser();
                    if (ProcessCacheList != null)
                    {
                        ProcessCacheList(parser.Parse(caches));
                    }
                }
            };
            client.DownloadStringAsync(new Uri(sUrl));
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
    }
}
