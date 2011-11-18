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
        private Action<List<Cache>> setPushpinsOnMap;
        private WebClient client;
        private int id;
        
        public Action<List<Cache>> SetPushpinsOnMap
        {
            get
            {
                return this.setPushpinsOnMap;
            }
            set
            {
                this.setPushpinsOnMap = value;
            }
        }
      
        public GeocahingSuApiManager()
        {
            Random random = new Random();
            this.id = random.Next(100000000);
            this.client = new WebClient();
            this.client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
        }

        public void GetCacheList(double lngmax, double lgnmin, double latmax, double latmin)
        {
            string sUrl = "http://www.geocaching.su/pages/1031.ajax.php?exactly=1&lngmax=" + lngmax +
                "&lngmin=" + lgnmin + "&latmax=" + latmax + "&latmin=" + latmin + "&id=" + this.id
                + "&geocaching=f1fadbc82d0156ae0f81f7d5e0b26bda";
            client.DownloadStringAsync(new Uri(sUrl));
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                XElement caches = XElement.Parse(e.Result);
                CacheParser parser = new CacheParser();
                if (SetPushpinsOnMap != null)
                {
                    SetPushpinsOnMap(parser.Parse(caches));
                }
            }
        }
    }
}
