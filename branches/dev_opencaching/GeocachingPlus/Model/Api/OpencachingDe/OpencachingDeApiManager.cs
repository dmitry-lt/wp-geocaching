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
                    var downloadedCaches = new List<Cache>();
                    string patternCount = "count=\"(.*?)\"";
                    var strCount = Regex.Matches(result, patternCount, RegexOptions.Singleline)[0].Value;
                    var count = Convert.ToInt32(strCount.Substring(7, strCount.Length - 1 - 7));
                    string patternCache = "<c(.*?)/>";
                    for (int i = 0; i < count; i++)
                    {
                        var cacheInfo = Regex.Matches(result, patternCache, RegexOptions.Singleline)[i].Value;
                        string patternCacheId = "wp=\"(.*?)\""; 
                        string strCacheId = Regex.Matches(result, patternCacheId, RegexOptions.Singleline)[0].Value;
                        string cacheId = strCacheId.Substring(4, strCacheId.Length  -1 - 4);
                        string patternCacheLongitude = "lon=\"(.*?)\"";
                        string strCacheLongitude = Regex.Matches(result, patternCacheLongitude, RegexOptions.Singleline)[0].Value;
                        string cacheLongitude = strCacheLongitude.Substring(5, strCacheLongitude.Length - 1 - 5);
                        string patternCacheLatitude = "lat=\"(.*?)\"";
                        string strCacheLatitude = Regex.Matches(result, patternCacheLatitude, RegexOptions.Singleline)[0].Value;
                        string cacheLatitude = strCacheLatitude.Substring(5, strCacheLatitude.Length - 1 - 5);
                        string patternType = "type=\"(.*?)\"";
                        string strCacheType = Regex.Matches(result, patternType, RegexOptions.Singleline)[0].Value;
                        string cacheType = strCacheType.Substring(6, strCacheType.Length - 1 - 6);
                        downloadedCaches.Add(
                            new OpencachingDeCache()
                            {
                                Id = cacheId,
                                //Name = ,
                                Location = new
                                    GeoCoordinate()
                                {
                                    Latitude = Convert.ToDouble(cacheLatitude, CultureInfo.InvariantCulture),
                                    Longitude = Convert.ToDouble(cacheLongitude, CultureInfo.InvariantCulture),
                                },
                                Type = GetType(cacheType)
                            });
                    }
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
                    processCaches(list);
                }
                RequestCounter.LiveMap.RequestSucceeded();

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
            var sUrl = String.Format(CultureInfo.InvariantCulture, CacheDescriptionUrl, cache.Id);

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
                        //var description = String.Format("{0} ({1}) <br/><br/> {2}", parsedCache.name, cache.Id, parsedCache.description);
                       // processDescription(String.Format(PatternCacheDescription, WebBrowserHelper.ConvertExtendedASCII(description)));
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

        private OpencachingDeCache.Types GetType(string text)
        {
            if (text == null)
            {
                return OpencachingDeCache.Types.Unknown;
            }
            switch (text.ToLower())
            {
                case "2":
                    return OpencachingDeCache.Types.Traditional;
                case "3":
                    return OpencachingDeCache.Types.Multi;
                case "5":
                    return OpencachingDeCache.Types.Webcam;
                case "7":
                    return OpencachingDeCache.Types.Quiz;
                case "6":
                    return OpencachingDeCache.Types.Event;
                case "8": // don't know exactly
                    return OpencachingDeCache.Types.Math;
                case "9": // don't know exactly
                    return OpencachingDeCache.Types.Moving;
                case "10":
                    return OpencachingDeCache.Types.DriveIn;
                case "4":
                    return OpencachingDeCache.Types.Virtual;
                case "1":
                    return OpencachingDeCache.Types.Unknown;
                default:
                    return OpencachingDeCache.Types.Unknown;
            }
        }

        private const string CacheDescriptionUrl = "http://www.opencaching.de/viewcache.php?wp=";
        private const string PatternCacheDescription = "<span style=\"font-family: Verdana, sans-serif; font-size: 13px; color: #424242; line-height: 16px; -webkit-border-horizontal-spacing: 5px; -webkit-border-vertical-spacing: 5px\" class=\"Apple-style-span\">(.*?)</span>";
        //private const string CachesUrl = "http://www.opencaching.de/map2.php?mode=searchresult&compact=1&resultid={0}&lat1={1}&lat2={2}&lon1={3}&lon2={4}";

    }
}
