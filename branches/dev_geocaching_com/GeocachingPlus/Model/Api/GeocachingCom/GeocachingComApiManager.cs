using System;
using System.Collections.Generic;

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

        public void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lgnmin, double latmax, double latmin)
        {
            var viewport = new Viewport(lngmax, lgnmin, latmax, latmin);

            HashSet<Tile> tiles = Tile.GetTilesForViewport(viewport);

            foreach (Tile tile in tiles) {
                if (!Tile.TileCache.Contains(tile)) {
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

                    if (tile.Zoomlevel != 14) {
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

                    tile.RequestMapInfo(GCConstants.URL_MAP_INFO, parameters, GCConstants.URL_LIVE_MAP);

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
