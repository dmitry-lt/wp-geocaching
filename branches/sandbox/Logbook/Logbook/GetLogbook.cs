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
    public class JsonStringSerializer
    {
        public static T Deserialize<T>(string strData) where T : class
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strData));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            T tRet = (T)ser.ReadObject(ms);
            ms.Close();
            return (tRet);
        }
    }

    public class GetLogbook
    {
        //public static T Deserialize<T>(string json)
        //{
        //    T obj = Activator.CreateInstance<T>();
        //    MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //    obj = (T)serializer.ReadObject(ms);
        //    ms.Close();
        //    ms.Dispose();
        //    return obj;
        //}

        public void FetchCacheDetails(Action<String> processLogbook, String cacheId)
        {
            string sUrl = InfoUrl + cacheId;
            var client = new WebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;
                //String pattern1 = "initalLogs = {.*[({.*{.*}.*[]})*]}";
                String pattern1 = "initalLogs = (\\{.+\\});";
               //String pattern2 = "{.*[({.*{.*}.*[]})*]}"; 
                String pattern2 = "(\\{.+\\});";
                var str1 = Regex.Matches(e.Result, pattern1, RegexOptions.Singleline)[0].Value;
                var str2 = Regex.Matches(str1, pattern2, RegexOptions.Singleline)[0].Value;
                var str3 = Regex.Replace(str2, "null", "\"null\"");
                var str4 = Regex.Replace(str3, "\"Images\":[.*]", "\"Images\":\"[]\"");
                
                LogbookInfo logbook = JsonStringSerializer.Deserialize<LogbookInfo>(str4);

                //LogbookInfo res = Deserialize<LogbookInfo>(str4);
                if (null != processLogbook)
                {
                   // processLogbook(str4);
                   //var logbook = Regex.Matches(e.Result, patternLogbook, RegexOptions.Multiline)[0].Value;
                    //processLogbook(logbook);
                }
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        private const string InfoUrl = "http://www.geocaching.com/seek/cache_details.aspx?wp=";

       // String patternLogbook = "<initalLogs = {\"status\":\"success\", \"data\": [{>(.*)<}>";
    }
}
