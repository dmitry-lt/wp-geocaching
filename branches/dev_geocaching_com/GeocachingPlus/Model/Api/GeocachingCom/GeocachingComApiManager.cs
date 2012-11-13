using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class GeocachingComApiManager : IApiManager
    {
        private double MilliTimeStamp()
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = DateTime.UtcNow;
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }

        private void DownloadCachesCompleted(Tile tile, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null) return;

            var jsonResult = e.Result;

            if (!String.IsNullOrWhiteSpace(jsonResult))
            {
                var nameCache = new Dictionary<string, string>(); // JSON id, cache name

                var parsedData = (GeocachingComApiCaches)JsonConvert.DeserializeObject(jsonResult, typeof(GeocachingComApiCaches));

                var keys = parsedData.keys;

                var positions = new Dictionary<string, List<UTFGridPosition>>(); // JSON id as key
                for (var i = 1; i < keys.Length; i++)
                {
                    // index 0 is empty
                    var key = keys[i];
                    if (!String.IsNullOrWhiteSpace(key))
                    {
                        var pos = UTFGridPosition.FromString(key);

                        var dataForKey = parsedData.data[key];
                        foreach (var c in dataForKey)
                        {
                            var id = c.i;
                            if (!nameCache.ContainsKey(id))
                            {
                                nameCache.Add(id, c.n);
                            }

                            if (!positions.ContainsKey(id))
                            {
                                positions.Add(id, new List<UTFGridPosition>());
                            }

                            positions[id].Add(pos);
                        }

                    }
                }

                var caches = new HashSet<Cache>();

                foreach (var id in positions.Keys)
                {
                    List<UTFGridPosition> pos = positions[id];
                    UTFGridPosition xy = UTFGrid.GetPositionInGrid(pos);
                    var cache = new GeocachingComCache()
                                    {
                                        Id = id,
                                        Name = nameCache[id],
                                        Location = tile.GetCoord(xy),
                                        ReliableLocation = false,
                                    };

                    // TODO: repeated entries
                    caches.Add(cache);

                    /*
                    cgCache cache = new cgCache();
                    cache.setDetailed(false);
                    cache.setReliableLatLon(false);
                    cache.setGeocode(id);
                    cache.setName(nameCache.get(id));
                    cache.setZoomlevel(tile.getZoomlevel());
                    cache.setCoords(tile.getCoord(xy));
                    if (strategy.flags.contains(StrategyFlag.PARSE_TILES) && positions.size() < 64 && bitmap != null) {
                        // don't parse if there are too many caches. The decoding would return too much wrong results
                        IconDecoder.parseMapPNG(cache, bitmap, xy, tile.getZoomlevel());
                    } else {
                        cache.setType(CacheType.UNKNOWN);
                    }
                    boolean exclude = false;
                    if (Settings.isExcludeMyCaches() && (cache.isFound() || cache.isOwn())) { // workaround for BM
                        exclude = true;
                    }
                    if (Settings.isExcludeDisabledCaches() && cache.isDisabled()) {
                        exclude = true;
                    }
                    if (Settings.getCacheType() != CacheType.ALL && Settings.getCacheType() != cache.getType() && cache.getType() != CacheType.UNKNOWN) { // workaround for BM
                        exclude = true;
                    }
                    if (!exclude) {
                        searchResult.addCache(cache);
                    }
                    */

                }
            }
        }

        public void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lgnmin, double latmax, double latmin)
        {
            var viewport = new Viewport(lngmax, lgnmin, latmax, latmin);

            var tiles = Tile.GetTilesForViewport(viewport);

            foreach (Tile tile in tiles)
            {
                if (!Tile.TileCache.Contains(tile))
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

                    // The PNG must be requested first, otherwise the following request would always return with 204 - No Content
                    /*
                    Bitmap bitmap = Tile.requestMapTile(params);
                    */

                    // Check bitmap size
                    /*
                    if (bitmap != null && (bitmap.getWidth() != Tile.TILE_SIZE ||
                            bitmap.getHeight() != Tile.TILE_SIZE)) {
                        bitmap.recycle();
                        bitmap = null;
                    }
                    */

                    tile.RequestMapInfo(DownloadCachesCompleted, GCConstants.URL_MAP_INFO, parameters, GCConstants.URL_LIVE_MAP);

                    /*
                    String data = Tile.requestMapInfo(GCConstants.URL_MAP_INFO, params, GCConstants.URL_LIVE_MAP);
                    if (StringUtils.isEmpty(data)) {
                        Log.e("GCBase.searchByViewport: No data from server for tile (" + tile.getX() + "/" + tile.getY() + ")");
                    } else {
                        final SearchResult search = GCMap.parseMapJSON(data, tile, bitmap, strategy);
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
                }

            }

        }

        public void FetchCacheDetails(Action<string> processDescription, Action<string> processLogbook, Action<List<string>> processPhotoUrls, Cache cache)
        {
            throw new NotImplementedException();
        }
    }
}
