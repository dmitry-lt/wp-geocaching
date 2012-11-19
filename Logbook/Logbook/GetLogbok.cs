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
using System.Text.RegularExpressions;

namespace Logbook
{
    public class GetLogbook
    {
        public void FetchCacheDetails(Action<String> processLogbook, String cacheId)
        {
            string sUrl = InfoUrl + cacheId;
            var client = new WebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;
                String patternLogbook = "initalLogs = (\\{.+\\});";
                if (null != processLogbook)
                {
                   var logbook = Regex.Matches(e.Result, patternLogbook, RegexOptions.Singleline)[0].Value;
                    processLogbook(logbook);
                }
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        private const string InfoUrl = "http://www.geocaching.com/seek/cache_details.aspx?wp=";
    }
}
