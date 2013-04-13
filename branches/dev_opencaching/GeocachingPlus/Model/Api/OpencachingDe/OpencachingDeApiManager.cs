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
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Device.Location;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Xml.Linq;

namespace GeocachingPlus.Model.Api.OpencachingDe
{
    public class OpencachingDeApiManager : IApiManager
    {
        internal OpencachingDeApiManager()
        {
            Caches = new HashSet<Cache>();
        }

        public HashSet<Cache> Caches { get; private set; }

        public void Post(string address, string parameters, Action<string> onResponseGot)
        {
            try
            {
                Action<IAsyncResult> getRequestStreamCallback = (IAsyncResult asynchronousResult) =>
                {
                    var post = parameters;

                    try
                    {
                        var req = (HttpWebRequest)asynchronousResult.AsyncState;

                        // End the operation
                        var postStream = req.EndGetRequestStream(asynchronousResult);

                        // Convert the string into a byte array.
                        var postBytes = new System.Text.UTF8Encoding().GetBytes(post); // change to utf8 from ascii

                        // Write to the request stream.
                        postStream.Write(postBytes, 0, postBytes.Length);
                        postStream.Close();

                        AsyncCallback getResponseCallback = (IAsyncResult asynchResult) =>
                        {
                            var request = (HttpWebRequest)asynchResult.AsyncState;

                            // End the operation
                            try
                            {
                                using (var response = (HttpWebResponse)request.EndGetResponse(asynchResult))
                                {
                                    var rcode = response.StatusCode;
                                    Stream streamResponse;
                                    streamResponse = response.GetResponseStream();
                                    using (streamResponse)
                                    {
                                        using (var streamRead = new StreamReader(streamResponse))
                                        {
                                            var responseString = streamRead.ReadToEnd();
                                            onResponseGot(responseString);
                                        }
                                    }
                                }
                            }
                            catch (WebException ex)
                            {
                                onResponseGot(null);
                            }
                        };

                        // Start the asynchronous operation to get the response
                        req.BeginGetResponse(getResponseCallback, req);
                    }
                    catch (Exception ex)
                    {
                        onResponseGot(null);
                    }
                };

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(address);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";

                // start the asynchronous operation
                httpWebRequest.BeginGetRequestStream(new AsyncCallback(getRequestStreamCallback), httpWebRequest);
                RequestCounter.LiveMap.RequestSent();
            }
            catch (Exception ex)
            {

            }
        }

        public void Post(string address, Dictionary<string, string> parameters, Action<string> onResponseGot)
        {
            var stringParams = FormUrlParameterQuery(parameters);
            Post(address, stringParams, onResponseGot);
        }

        public static string FormUrlParameterQuery(Dictionary<string, string> parameters)
        {
            var result = "";
            var firstParameter = true;
            foreach (var k in parameters.Keys)
            {
                if (!firstParameter)
                {
                    result += "&";
                }
                result += k + "=" + parameters[k];
                firstParameter = false;
            }
            return result;
        }

        public void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lngmin, double latmax, double latmin)
        {
            Dictionary<string, string> parametrs1 = new Dictionary<string, string>();
            parametrs1.Add("showresult", "1");
            parametrs1.Add("expert", "0");
            parametrs1.Add("output", "map2");
            parametrs1.Add("utf8", "1");
            parametrs1.Add("skipqueryid", "1");
            parametrs1.Add("searchto", "searchbynofilter");
            parametrs1.Add("cachetype", "1;2;3;4;5;6;7;8;9;10");
            parametrs1.Add("cachesize", "1;2;3;4;5;6;7;8");
            parametrs1.Add("f_inactive", "1");
            parametrs1.Add("cache_attribs", "");
            parametrs1.Add("cache_attribs_not", "");

            Action<string> addCaches = delegate(string result)
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(result)))
                {
                    XDocument xmlResult = XDocument.Parse(result); 
                    OpencachingDeCacheParser parser = new OpencachingDeCacheParser();
                    var downloadedCaches = parser.Parse(xmlResult);
                    foreach (var p in downloadedCaches)
                    {
                        if (!Caches.Contains(p))
                        {
                            Caches.Add(p);
                        }
                    }

                    if (processCaches == null) return;
                    var list = (from cache in Caches
                                where ((cache.Location.Latitude <= latmax) &&
                                       (cache.Location.Latitude >= latmin) &&
                                       (cache.Location.Longitude <= lngmax) &&
                                       (cache.Location.Longitude >= lngmin))
                                select cache).ToList<Cache>();
    
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        processCaches(list);
                    });
                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    RequestCounter.LiveMap.RequestSucceeded();
                });

            };
            Action<string> mapRequest = delegate(string resultId)
            {
                Dictionary<string, string> parametrs = new Dictionary<string, string>();
                parametrs.Add("mode", "searchresult");
                parametrs.Add("resultid", resultId);
                parametrs.Add("lat1", latmin.ToString());
                parametrs.Add("lat2", latmax.ToString());
                parametrs.Add("lon1", lngmin.ToString());
                parametrs.Add("lon2", lngmax.ToString());

                Post("http://www.opencaching.de/map2.php", parametrs, addCaches);
            };
 
            Post("http://www.opencaching.de/search.php", parametrs1, mapRequest);
        }

        public void FetchCacheDetails(Action<string> processDescription, Action<string> processLogbook, Action<List<string>> processPhotoUrls, Action<string> processHint, Cache cache)
        {
            var sUrl = String.Format(CacheDescriptionUrl, cache.Id);

            var client = new WebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;

                var jsonResult = e.Result;

               // var serializer = new DataContractJsonSerializer(typeof(OpencachingDeApiCache));

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonResult)))
                {
                   // var parsedCache = (OpencachingDeApiCache)serializer.ReadObject(ms);

                    // description
                    if (null != processDescription)
                    {
                        var description = Regex.Matches(e.Result, PatternCacheDescription, RegexOptions.Singleline)[0].Value;
                        processDescription(description);
                    }

                    // logs
                    if (null != processLogbook)
                    {
                        
                    }

                    // photos
                    if (null != processPhotoUrls)
                    {
                        
                    }
                }
            };

            client.DownloadStringAsync(new Uri(sUrl)); 
        }

        /*public void FetchCacheLog(Action<string> processLogbook, Cache cache)
        {
            var sUrl = String.Format(CultureInfo.InvariantCulture, CacheLogUrl, cache.Id);

            var client = new WebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;

                var jsonResult = e.Result;

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonResult)))
                {
                    // logs
                    if (null != processLogbook)
                    {

                    }
                }
            };

            client.DownloadStringAsync(new Uri(sUrl)); 

        }*/

        private const string CacheDescriptionUrl = "http://www.opencaching.de/viewcache.php?wp={0}";
        private const string CacheLogUrl = "http://www.opencaching.de/viewlogs.php?cacheid=";
        private const string PatternCacheDescription = "<div class=\"content2-container cachedesc\">(.*?)</div>";
        private const string PatternCacheLog = "<div class=\"content-txtbox-noshade\"><div class=\"logs\" id=\"log874219\"><p class=\"content-title-noshade-size1\" style=\"display:inline;\"><img src=\"resource2/ocstyle/images/log/16x16-found.png\" alt=\"Found\" />(.*?)<a href=\"viewprofile.php?userid=120056\">(.*?)</a>(.*?)</p><div class=\"viewcache_log-content\" style=\"margin-top: 15px;\"><p><p>(.*?)<br /><br />(.*?)</p></p></div></div></div>";
       
    }
}
