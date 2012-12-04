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
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;


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
                String pattern1 = "initalLogs = (\\{.+?\\});";
                String pattern2 = "(\\{.+\\});";
                var str1 = Regex.Matches(e.Result, pattern1, RegexOptions.Singleline)[0].Value;
                var str2 = Regex.Matches(str1, pattern2, RegexOptions.Singleline)[0].Value;
                var str3 = Regex.Replace(str2, "null", "\"null\"", RegexOptions.Singleline);
                var str4 = Regex.Replace(str3, "\\[\\]", "\"[]\"", RegexOptions.Singleline).ToString();
                var str5 = str4.Substring(0, str4.Length - 1);

                LogbookInfo logbook = Activator.CreateInstance<LogbookInfo>();
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(str5));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(logbook.GetType());
                logbook = (LogbookInfo)serializer.ReadObject(ms);
                ms.Close();
                ms.Dispose();
                
                if (null != processLogbook)
                {
                    processLogbook(logbook.outPut());      
                }
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        private const string InfoUrl = "http://www.geocaching.com/seek/cache_details.aspx?wp=";
    }
}
