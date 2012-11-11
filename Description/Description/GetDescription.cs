using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;

namespace Description
{
    class GetDescription
    {
         public void FetchCacheDetails(String cacheId)
        {
            string sUrl = InfoUrl + cacheId;
            var client = new WebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;
                MainPage page = new MainPage();
                page.textBox1.Text = e.Result;
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        private const string InfoUrl = "http://www.geocaching.com/seek/cache_details.aspx?wp=";
    }
}
