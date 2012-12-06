using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using ComputerBeacon.Json;
using GeocachingPlus.ViewModel;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class GeocachingComApiManager : IApiManager
    {
        private readonly TileCache _tileCache = new TileCache();

        internal GeocachingComApiManager()
        {
            Caches = new HashSet<Cache>();
        }

        public HashSet<Cache> Caches { get; private set; }

        private double MilliTimeStamp()
        {
            var d1 = new DateTime(1970, 1, 1);
            var d2 = DateTime.UtcNow;
            var ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }

/*
        #region tiles to process queue

        private readonly object _lock = new object();

        private readonly List<Tile> _tilesToProcess = new List<Tile>();

        private void SetTilesToProcess(IEnumerable<Tile> tiles)
        {
            lock (_lock)
            {
                _tilesToProcess.Clear();
                _tilesToProcess.AddRange(tiles);
            }
        }

        private Tile GetTileToProcess()
        {
            lock (_lock)
            {
                Tile tile = null;
                if (_tilesToProcess.Any())
                {
                    tile = _tilesToProcess.First();
                    _tilesToProcess.Remove(tile);
                }
                return tile;
            }
        }

        #endregion
*/

        private readonly object _lock = new object();
        private int _fetchCachesCallNumber;
        private int InitNextFetchCachesCallNumber()
        {
            lock (_lock)
            {
                return ++_fetchCachesCallNumber;
            }
        }

        private int GetFetchCachesCallNumber()
        {
            lock (_lock)
            {
                return _fetchCachesCallNumber;
            }
        }

        private void FetchTile(int fetchCachesCallNumber, ICollection<Tile> tiles, int tileIndex, Action<List<Cache>> processCaches, double lngmax, double lngmin, double latmax, double latmin)
        {
            if (tileIndex >= tiles.Count() || fetchCachesCallNumber != GetFetchCachesCallNumber())
            {
                return;
            }

            var tile = tiles.ElementAt(tileIndex);

            if (!_tileCache.Contains(tile))
            {
                var parameters = new Dictionary<string, string>()
                    {
                            {"x", tile.TileX + ""},
                            {"y", tile.TileY + ""},
                            {"z", tile.Zoomlevel + ""},
                            {"ep", "1"},
                    };

                /*
                if (tokens != null) {
                    params.put("k", tokens[0], "st", tokens[1]);
                }
                if (Settings.isExcludeMyCaches()) { // works only for PM
                    params.put("hf", "1", "hh", "1"); // hide found, hide hidden
                }
                if (Settings.getCacheType() == CacheType.TRADITIONAL) {
                    params.put("ect", "9,5,3,6,453,13,1304,137,11,4,8,1858"); // 2 = tradi 3 = multi 8 = mystery
                } else if (Settings.getCacheType() == CacheType.MULTI) {
                    params.put("ect", "9,5,2,6,453,13,1304,137,11,4,8,1858");
                } else if (Settings.getCacheType() == CacheType.MYSTERY) {
                    params.put("ect", "9,5,3,6,453,13,1304,137,11,4,2,1858");
                }
                */

                if (tile.Zoomlevel != 14)
                {
                    parameters.Add("_", MilliTimeStamp() + "");
                }

                // TODO: other types t.b.d

                var currentTile = tile;

                // The PNG must be requested first, otherwise the following request would always return with 204 - No Content
                Action<WriteableBitmap> processBitmap = bitmap =>
                {
                    if (null == bitmap)
                    {
                        RequestCounter.LiveMap.RequestFailed();
                    }
                    else
                    {
                        RequestCounter.LiveMap.RequestSucceeded();
                    }

                    // Check bitmap size
                    if (bitmap != null && (bitmap.PixelWidth != Tile.TILE_SIZE || bitmap.PixelHeight != Tile.TILE_SIZE))
                    {
                        bitmap = null;
                    }

                    Action<DownloadStringCompletedEventArgs> downloadCachesCompleted =
                        (e) =>
                        {
                            if (e.Error != null)
                            {
                                RequestCounter.LiveMap.RequestFailed();
                            }
                            else
                            {
                                var jsonResult = e.Result;

                                if (!String.IsNullOrWhiteSpace(jsonResult))
                                {
                                    var nameCache = new Dictionary<string, string>(); // JSON id, cache name

                                    var parsedData = new JsonObject(jsonResult);

                                    const string keysId = "keys";
                                    const string dataId = "data";

                                    var keys = (parsedData.ContainsKey(keysId) ? parsedData[keysId] : null) as JsonArray;
                                    var data = (parsedData.ContainsKey(dataId) ? parsedData[dataId] : null) as JsonObject;

                                    var positions = new Dictionary<string, List<UTFGridPosition>>();
                                    // JSON id as key
                                    if (null != keys && null != data)
                                    {
                                        for (var i = 1; i < keys.Count; i++)
                                        {
                                            // index 0 is empty
                                            var keyObject = keys[i];
                                            var key = keyObject == null ? null : keyObject.ToString();
                                            if (!String.IsNullOrWhiteSpace(key))
                                            {
                                                var pos = UTFGridPosition.FromString(key);

                                                var dataForKey = data[key] as JsonArray;
                                                if (null != dataForKey)
                                                {
                                                    foreach (JsonObject c in dataForKey)
                                                    {
                                                        var id = c["i"].ToString();
                                                        if (!nameCache.ContainsKey(id))
                                                        {
                                                            nameCache.Add(id, c["n"].ToString());
                                                        }

                                                        if (!positions.ContainsKey(id))
                                                        {
                                                            positions.Add(id, new List<UTFGridPosition>());
                                                        }

                                                        positions[id].Add(pos);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    var caches = new List<Cache>();

                                    foreach (var id in positions.Keys)
                                    {
                                        var pos = positions[id];
                                        var xy = UTFGrid.GetPositionInGrid(pos);
                                        var cache = new GeocachingComCache()
                                        {
                                            Id = id,
                                            Name = nameCache[id],
                                            Location = currentTile.GetCoord(xy),
                                            ReliableLocation = false,
                                        };

                                        IconDecoder.parseMapPNG(cache, bitmap, xy, currentTile.Zoomlevel);

                                        caches.Add(cache);
                                    }

                                    _tileCache.Add(currentTile);

                                    foreach (var p in caches)
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
                            }

                            FetchTile(fetchCachesCallNumber, tiles, tileIndex + 1, processCaches, lngmax, lngmin, latmax, latmin);
                        };

                    if (fetchCachesCallNumber != GetFetchCachesCallNumber())
                    {
                        return;
                    }

                    RequestCounter.LiveMap.RequestSent();

                    currentTile.RequestMapInfo(downloadCachesCompleted, GCConstants.URL_MAP_INFO, parameters, GCConstants.URL_LIVE_MAP);

                    /*
                    String data = Tile.requestMapInfo(GCConstants.URL_MAP_INFO, params, GCConstants.URL_LIVE_MAP);
                    if (StringUtils.isEmpty(data)) {
                        Log.e("GCBase.searchByViewport: No data from server for tile (" + tile.getX() + "/" + tile.getY() + ")");
                    } else {
                         SearchResult search = GCMap.parseMapJSON(data, tile, bitmap, strategy);
                        if (search == null || CollectionUtils.isEmpty(search.getGeocodes())) {
                            Log.e("GCBase.searchByViewport: No cache parsed for viewport " + viewport);
                        }
                        else {
                            searchResult.addGeocodes(search.getGeocodes());
                        }
                        Tile.Cache.add(tile);
                    }
                    */

                    // release native bitmap memory
                    /*
                    if (bitmap != null) {
                        bitmap.recycle();
                    }
                    */

                };

                RequestCounter.LiveMap.RequestSent();

                Tile.RequestMapTile(processBitmap, parameters);
            }
        }

        private bool _triedToLogin;
        private void TryToLogin()
        {
            if (!_triedToLogin)
            {
                _triedToLogin = true;
                var key = typeof (GeocachingComLoginPageViewModel).Name;
                if (((App) Application.Current).Resources.Contains(key))
                {
                    var obj = ((App) Application.Current).Resources[key];
                    if (obj is GeocachingComLoginPageViewModel)
                    {
                        var vm = obj as GeocachingComLoginPageViewModel;
                        vm.Login();
                    }
                }
            }
        }

        public void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lngmin, double latmax, double latmin)
        {
            TryToLogin();
            if (processCaches == null) return;
            var cacheList = (from cache in Caches
                        where ((cache.Location != null) &&
                               (cache.Location.Latitude <= latmax) &&
                               (cache.Location.Latitude >= latmin) &&
                               (cache.Location.Longitude <= lngmax) &&
                               (cache.Location.Longitude >= lngmin))
                        select cache).ToList<Cache>();
            processCaches(cacheList);

            var viewport = new Viewport(lngmax, lngmin, latmax, latmin);

            var tiles = Tile.GetTilesForViewport(viewport);

            FetchTile(InitNextFetchCachesCallNumber(), tiles, 0, processCaches, lngmax, lngmin, latmax, latmin);
        }


        private const string GeocachingComUrl = "http://www.geocaching.com";
        private const string GeocachingComSeekUrl = GeocachingComUrl + "/seek/";
        private const string InfoUrl = GeocachingComSeekUrl + "cache_details.aspx?wp=";
        private const string PatternShortdesc = "<span id=\"ctl00_ContentBody_ShortDescription\">(.*?)</span>\\s*</div>";
        private const string PatternDesc = "<span id=\"ctl00_ContentBody_LongDescription\">(.*?)</span>\\s*</div>\\s*<p>\\s*</p>\\s*<p id=\"ctl00_ContentBody_hints\">";
        private const string PatternImg = "<img.*?src=\"(.*?)\".*?/>";
        private const string PatternType = "<img src=\"[^\"]*/WptTypes/\\d+\\.gif\" alt=\"([^\"]+?)\"[^>]*>";
        private const string PatternSpoilerImage = "<a href=\"(http://img\\.geocaching\\.com/cache/[^.]+\\.jpe?g)\"[^>]+><img[^>]+><span>([^<]+)</span></a>(?:<br />([^<]+)<br /><br />)?";
        private const string PatternLatLon = "<span id=\"uxLatLon\" style=\"font-weight:bold;\"[^>]*>(.*?)</span>";
        private const string PatternHint = "<div id=\"div_hint\"[^>]*>(.*?)</div>";
        private const string PatternLinebreak = "<(br|p)[^>]*>";

        private GeocachingComCache.Types GetType(string altText)
        {
            if (altText == null)
            {
                return GeocachingComCache.Types.UNKNOWN;
            }
            switch (altText.ToLower())
            {
                case "traditional cache":
                    return GeocachingComCache.Types.TRADITIONAL;
                case "multi-cache":
                    return GeocachingComCache.Types.MULTI;
                case "unknown cache":
                    return GeocachingComCache.Types.UNKNOWN;
                case "letterbox hybrid":
                    return GeocachingComCache.Types.LETTERBOX;
                case "event cache":
                    return GeocachingComCache.Types.EVENT;
                case "mega-event cache":
                    return GeocachingComCache.Types.MEGA_EVENT;
                case "earthcache":
                    return GeocachingComCache.Types.EARTH;
                case "cache in trash out event":
                    return GeocachingComCache.Types.CITO;
                case "webcam cache":
                    return GeocachingComCache.Types.WEBCAM;
                case "virtual cache":
                    return GeocachingComCache.Types.VIRTUAL;
                case "wherigo cache":
                    return GeocachingComCache.Types.WHERIGO;
                case "lost & found":
                    return GeocachingComCache.Types.LOSTANDFOUND;
                case "project ape cache":
                    return GeocachingComCache.Types.PROJECT_APE;
                case "groundspeak hq":
                    return GeocachingComCache.Types.GCHQ;
                case "gps cache exhibit":
                    return GeocachingComCache.Types.GPS_EXHIBIT;
                default:
                    return GeocachingComCache.Types.UNKNOWN;
            }
        }

        public void FetchCacheDetails(Action<string> processDescription, Action<string> processLogbook, Action<List<string>> processPhotoUrls, Action<string> processHint, Cache cache)
        {
            if (null == processDescription || null == processLogbook || null == processPhotoUrls || null == processHint)
            {
                return;
            }

            var sUrl = InfoUrl + cache.Id;
            var client = new ExtendedWebClient();

            Action<string> downloadStringCompleted = initialHtml =>
            {
                if (initialHtml == null) return;

                var html = WebBrowserHelper.ConvertExtendedASCII(initialHtml);

                var shortDescription = "";
                var matchesShortdesc = Regex.Matches(html, PatternShortdesc, RegexOptions.Singleline);
                if (matchesShortdesc.Count == 1)
                {
                    shortDescription = matchesShortdesc[0].Groups[1].Value;
                }
                else
                {
                    //TODO: log error
                    var cacheId = cache.Id;
                }

                var description = "";
                var matches = Regex.Matches(html, PatternDesc, RegexOptions.Singleline);
                if (matches.Count == 1)
                {
                    description = matches[0].Groups[1].Value;
                }
                else
                {
                    //TODO: log error
                    var cacheId = cache.Id;
                }

                var totalDescription = String.Format("{0} <br/><br/> {1}", shortDescription, description);

                // cache type
                var cacheType = GeocachingComCache.Types.UNKNOWN;
                var typeMatches = Regex.Matches(initialHtml, PatternType, RegexOptions.Singleline);
                if (typeMatches.Count == 1)
                {
                    var typeString = typeMatches[0].Groups[1].Value;
                    cacheType = GetType(typeString);
                }
                else
                {
                    //TODO: log error
                    var cacheId = cache.Id;
                }

                // cache location
                GeoCoordinate cacheLocation = null;
                var locationMatches = Regex.Matches(initialHtml, PatternLatLon, RegexOptions.Singleline);
                if (locationMatches.Count > 0)
                {
                    var locationString = locationMatches[0].Groups[1].Value;
                    cacheLocation = GeopointParser.Parse(locationString);
                }

                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                        {
                            var gcCache = ((GeocachingComCache)cache);
                            if (GeocachingComCache.Types.UNKNOWN != cacheType)
                            {
                                gcCache.Type = cacheType;
                            }
                            if (null != cacheLocation)
                            {
                                gcCache.Location = cacheLocation;
                                gcCache.ReliableLocation = true;
                            }

                            var finalDesc = String.Format(
                                "{0} ({1}) <br/><br/> {2}", 
                                WebBrowserHelper.ConvertExtendedASCII(cache.Name), 
                                cache.Id,
                                totalDescription);

                            processDescription(finalDesc);
                        });

                // TODO: implement logbook
                Deployment.Current.Dispatcher.BeginInvoke(() => processLogbook(""));


                // searching for images in description
                var photoUrls = new List<string>();
                var urls = Regex.Matches(totalDescription, PatternImg);

                for (var i = 0; i < urls.Count; i++)
                {
                    var photoUrl = urls[i].Groups[1].Value;
                    if (!photoUrl.StartsWith("http://") && !photoUrl.StartsWith("https://"))
                    {
                        if (photoUrl.StartsWith("/"))
                        {
                            photoUrl = GeocachingComUrl + photoUrl;
                        }
                        else
                        {
                            photoUrl = GeocachingComSeekUrl + photoUrl;
                        }
                    }
                    photoUrls.Add(photoUrl);
                }

                // spoiler images
                var spoilerImageUrls = Regex.Matches(html, PatternSpoilerImage);
                for (var i = 0; i < spoilerImageUrls.Count; i++)
                {
                    var photoUrl = spoilerImageUrls[i].Groups[1].Value;
                    if (!photoUrl.StartsWith("http://") && !photoUrl.StartsWith("https://"))
                    {
                        if (photoUrl.StartsWith("/"))
                        {
                            photoUrl = GeocachingComUrl + photoUrl;
                        }
                        else
                        {
                            photoUrl = GeocachingComSeekUrl + photoUrl;
                        }
                    }
                    photoUrls.Add(photoUrl);
                }

                Deployment.Current.Dispatcher.BeginInvoke(() => processPhotoUrls(photoUrls));

                var hint = "";
                var hintUrls = Regex.Matches(initialHtml, PatternHint, RegexOptions.Singleline);
                for (var i = 0; i < hintUrls.Count; i++)
                {
                    hint += hintUrls[i].Groups[1].Value;
                }

                // replace linebreak and paragraph tags
                var breaks = Regex.Matches(hint, PatternLinebreak, RegexOptions.Singleline);
                for (var i = 0; i < breaks.Count; i++)
                {
                    var lineBreak = breaks[i].Groups[0].Value;
                    hint = hint.Replace(lineBreak, "\n");
                }
                hint = hint.Trim();

                hint = CryptUtils.Rot13(hint);

                Deployment.Current.Dispatcher.BeginInvoke(() => processHint(hint));

            };
            client.Get(sUrl, downloadStringCompleted);
        }

        private class TileCache
        {
            private readonly LruCache<int, Tile> _tileCache = new LruCache<int, Tile>(64);

            /*
                        public static void RemoveFromTileCache(GeoCoordinate point) {
                            if (point != null) {
                                var tiles = tileCache.GetValues();
                                foreach (Tile tile in tiles) {
                                    if (tile.ContainsPoint(point)) {
                                        tileCache.remove(tile.GetHashCode());
                                    }
                                }
                            }
                        }
            */

            public bool Contains(Tile tile)
            {
                return _tileCache.ContainsKey(tile.GetHashCode());
            }

            public void Add(Tile tile)
            {
                _tileCache.Add(tile.GetHashCode(), tile);
            }
        }
    }
}
