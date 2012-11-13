using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using GeocachingPlus.Model.Api.OpenCachingCom;
using Microsoft.Xna.Framework;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class Tile
    {
        private const int ZoomlevelMax = 18;
        private const int ZoomlevelMin = 0;

        public const  int TILE_SIZE = 256;

        private static readonly int[] NumberOfTiles = new int[ZoomlevelMax - ZoomlevelMin + 1];
        private static readonly int[] NumberOfPixels = new int[ZoomlevelMax - ZoomlevelMin + 1];


        public int TileX { get; set; }
        public int TileY { get; set; }
        public int Zoomlevel { get; set; }

        public Viewport Viewport { get; set; }

        static Tile()
        {
            for (var z = ZoomlevelMin; z <= ZoomlevelMax; z++)
            {
                NumberOfTiles[z] = 1 << z;
                NumberOfPixels[z] = TILE_SIZE * 1 << z;
            }
        }

        public Tile(GeoCoordinate origin, int zoomlevel)
        {
            Zoomlevel = Math.Max(Math.Min(zoomlevel, ZoomlevelMax), ZoomlevelMin);

            TileX = CalcX(origin);
            TileY = CalcY(origin);

            Viewport = new Viewport(GetCoord(new UTFGridPosition(0, 0)), GetCoord(new UTFGridPosition(63, 63)));
        }

        /**
         * Calculate latitude/longitude for a given x/y position in this tile.
         * 
         * @see <a
         *      href="http://developers.cloudmade.com/projects/tiles/examples/convert-coordinates-to-tile-numbers">Cloudmade</a>
         */
        public GeoCoordinate GetCoord(UTFGridPosition pos) {

            double pixX = TileX * TILE_SIZE + pos.x * 4;
            double pixY = TileY * TILE_SIZE + pos.y * 4;

            double lonDeg = ((360.0 * pixX) / NumberOfPixels[Zoomlevel]) - 180.0;
            double latRad = Math.Atan(Math.Sinh(Math.PI * (1 - 2 * pixY / NumberOfPixels[Zoomlevel])));
            return new GeoCoordinate(ConvertToDegrees(latRad), lonDeg);
        }

        /**
         * Calculate the tile for a Geopoint based on the Spherical Mercator.
         *
         * @see <a
         *      href="http://developers.cloudmade.com/projects/tiles/examples/convert-coordinates-to-tile-numbers">Cloudmade</a>
         */
        private int CalcX(GeoCoordinate origin)
        {
            return (int)((origin.Longitude + 180.0) / 360.0 * NumberOfTiles[Zoomlevel]);
        }

        /**
         * Calculate the tile for a Geopoint based on the Spherical Mercator.
         *
         */
        private int CalcY(GeoCoordinate origin)
        {
            // double latRad = Math.toRadians(origin.getLatitude());
            // return (int) ((1 - (Math.log(Math.tan(latRad) + (1 / Math.cos(latRad))) / Math.PI)) / 2 * numberOfTiles);

            // Optimization from Bing
            var sinLatRad = Math.Sin(ConvertToRadians(origin.Latitude));
            return (int)((0.5 - Math.Log((1 + sinLatRad) / (1 - sinLatRad)) / (4 * Math.PI)) * NumberOfTiles[Zoomlevel]);
        }

        private double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public static double ConvertToDegrees(double angrad) {
    	    return angrad * 180.0 / Math.PI;
        }


        /**
         * Calculates the inverted hyperbolic sine
         * (after Bronstein, Semendjajew: Taschenbuch der Mathematik
         *
         * @param x
         * @return
         */
        private static double asinh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x + 1.0));
        }

        private static double tanGrad(double angleGrad)
        {
            return Math.Tan(angleGrad / 180.0 * Math.PI);
        }

        /**
         * Calculates the maximum possible zoom level where the supplied points
         * are covered by adjacent tiles on the east/west axis.
         * The order of the points (left/right) is irrelevant.
         *
         * @param left
         *            First point
         * @param right
         *            Second point
         * @return
         */
        public static int CalcZoomLon(GeoCoordinate left, GeoCoordinate right)
        {

            int zoom = (int)Math.Floor(
                    Math.Log(360.0 / Math.Abs(left.Longitude - right.Longitude))
                            / Math.Log(2)
                    );

            Tile tileLeft = new Tile(left, zoom);
            Tile tileRight = new Tile(right, zoom);

            if (tileLeft.TileX == tileRight.TileX)
            {
                zoom += 1;
            }

            return Math.Min(zoom, ZoomlevelMax);
        }

        /**
         * Calculates the maximum possible zoom level where the supplied points
         * are covered by adjacent tiles on the north/south axis.
         * The order of the points (bottom/top) is irrelevant.
         *
         * @param bottom
         *            First point
         * @param top
         *            Second point
         * @return
         */
        public static int CalcZoomLat(GeoCoordinate bottom, GeoCoordinate top)
        {

            int zoom = (int)Math.Ceiling(
                    Math.Log(2.0 * Math.PI /
                            Math.Abs(
                                    asinh(tanGrad(bottom.Latitude))
                                            - asinh(tanGrad(top.Latitude))
                                    )
                            ) / Math.Log(2)
                    );

            Tile tileBottom = new Tile(bottom, zoom);
            Tile tileTop = new Tile(top, zoom);

            if (Math.Abs(tileBottom.TileY - tileTop.TileY) > 1)
            {
                zoom -= 1;
            }

            return Math.Min(zoom, ZoomlevelMax);
        }

        public static HashSet<Tile> GetTilesForViewport(GeoCoordinate bottomLeft, GeoCoordinate topRight)
        {
            var tiles = new HashSet<Tile>();
            int zoom = Math.Min(Tile.CalcZoomLon(bottomLeft, topRight), Tile.CalcZoomLat(bottomLeft, topRight));
            tiles.Add(new Tile(bottomLeft, zoom));
            tiles.Add(new Tile(new GeoCoordinate(bottomLeft.Latitude, topRight.Longitude), zoom));
            tiles.Add(new Tile(new GeoCoordinate(topRight.Latitude, bottomLeft.Longitude), zoom));
            tiles.Add(new Tile(topRight, zoom));
            return tiles;
        }

        public static HashSet<Tile> GetTilesForViewport(Viewport viewport) {
            return GetTilesForViewport(viewport.bottomLeft, viewport.topRight);
        }

        public bool ContainsPoint(GeoCoordinate point) {
            return Viewport.Contains(point);
        }

        public override String ToString() {
            return String.Format("({0}/{1}), zoom={2}", TileX, TileY, Zoomlevel);
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        private static string FormUrl(string baseUrl, Dictionary<string, string> parameters)
        {
            var urlString = baseUrl;
            var firstParameter = true;
            foreach (var k in parameters.Keys)
            {
                if (firstParameter)
                {
                    urlString += "?" + k + "=" + parameters[k];
                    firstParameter = false;
                }
                else
                {
                    urlString += "&" + k + "=" + parameters[k];
                }
            }
            return urlString;
        }

        /** Request .png image for a tile. */
        public static void RequestMapTile(Dictionary<string, string> parameters)
        {
            var urlString = FormUrl(GCConstants.URL_MAP_TILE, parameters);

            var client = new WebClient();

            client.DownloadStringAsync(new Uri(urlString));
        }

        /** Request JSON informations for a tile */
        public static void RequestMapInfo(String url, Dictionary<string, string> parameters, string referer) {
            var urlString = FormUrl(url, parameters);

            var client = new WebClient();
            client.Headers["Referer"] = referer;

            client.DownloadStringCompleted +=
                (sender, e) =>
                    {
                        if (e.Error != null) return;

                        var jsonResult = e.Result;
                    };

            client.DownloadStringAsync(new Uri(urlString));
        }



        public static class Cache {
            private static LruCache<int, Tile> tileCache = new LruCache<int, Tile>(64);

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

            public static bool Contains(Tile tile) {
                return tileCache.ContainsKey(tile.GetHashCode());
            }

            public static void Add(Tile tile) {
                tileCache.Add(tile.GetHashCode(), tile);
            }
        }

    }
}
