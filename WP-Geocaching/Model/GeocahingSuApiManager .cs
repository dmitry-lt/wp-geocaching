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
    /// IApiManager implementation for Geocaching.su
    /// </summary>
    public class GeocahingSuApiManager : IApiManager
    {
        private int id;

        public GeocahingSuApiManager()
        {
            Random random = new Random();
            this.id = random.Next(100000000);           
        }

        public void GetCacheList(Action<List<Cache>> ProcessCacheList, double lngmax, double lngmin, double latmax, double latmin)
        {
            string sUrl = "http://www.geocaching.su/pages/1031.ajax.php?exactly=1&lngmax=" + lngmax +
                "&lngmin=" + lngmin + "&latmax=" + latmax + "&latmin=" + latmin + "&id=" + this.id
                + "&geocaching=f1fadbc82d0156ae0f81f7d5e0b26bda";
            WebClient client = new WebClient();
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
    }
}
