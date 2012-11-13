using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Description
{
    class GetDescription
    {
         public void FetchCacheDetails(Action<String> processDescription, String cacheId)
        {
            string sUrl = InfoUrl + cacheId;
            var client = new WebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;
                var description = e.Result;
                //String patternDesc = "\"[0-9]*\"";
                //String patternDesc = @" (<span id="ctl00_ContentBody_LongDescription">) ((\w\s*<br />*)*) "</span>"";
                //Regex regex = new Regex(patternDesc);
                //Match match = regex.Match(e.Result);
                if (null != processDescription)
                {
                    //var description = match.ToString();
                    processDescription(description);
                    //processDescription(String.Format(description));
                }
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        private const string InfoUrl = "http://www.geocaching.com/seek/cache_details.aspx?wp=";
    }
}
