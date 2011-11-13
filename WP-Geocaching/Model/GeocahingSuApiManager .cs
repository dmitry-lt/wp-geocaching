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
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Device.Location;

namespace WP_Geocaching.Model
{
    /// <summary>
    /// IApiManager Implementation for Geocaching.su
    /// </summary>
    public class GeocahingSuApiManager : IApiManager
    {
        private WebClient client;
        List<Cache> cacheList;
        private int id;

        public GeocahingSuApiManager()
        {
            Random random = new Random();
            this.id = random.Next(100000000);
            this.client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);       
        }

        public void GetCacheList(double lngmax, double lgnmin, double latmax, double latmin)
        {
            string sUrl = "http://www.geocaching.su/pages/1031.ajax.php?lngmax=" + lngmax +
                "&lngmin=" + lgnmin + "&latmax=" + latmax + "&latmin=" + latmin + "&id=" + this.id;
            client_DownloadStringAsync(sUrl);

        }

        private void client_DownloadStringAsync(string url)
        {
            client.DownloadStringAsync(new Uri(url));
        }

        private void client_DownloadStringCompleted(object sender,  DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                XElement caches = XElement.Parse(e.Result);
                CacheParser parser = new CacheParser();
                this.cacheList = parser.Parse(caches);
            }
        }
    }
}
