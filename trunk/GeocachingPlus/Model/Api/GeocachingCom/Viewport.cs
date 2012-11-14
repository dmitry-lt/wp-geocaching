using System;
using System.Device.Location;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class Viewport
    {
        public GeoCoordinate center;
        public GeoCoordinate bottomLeft;
        public GeoCoordinate topRight;

        public Viewport(ICoordinates point1, ICoordinates point2)
        {
            GeoCoordinate gp1 = point1.GetCoords();
            GeoCoordinate gp2 = point2.GetCoords();
            this.bottomLeft = new GeoCoordinate(Math.Min(gp1.Latitude, gp2.Latitude), Math.Min(gp1.Longitude, gp2.Longitude));
            this.topRight = new GeoCoordinate(Math.Max(gp1.Latitude, gp2.Latitude), Math.Max(gp1.Longitude, gp2.Longitude));
            this.center = new GeoCoordinate((gp1.Latitude + gp2.Latitude) / 2, (gp1.Longitude + gp2.Longitude) / 2);
        }

        public Viewport(double gp1Longitude, double gp2Longitude, double gp1Latitude, double gp2Latitude)
        {
            this.bottomLeft = new GeoCoordinate(Math.Min(gp1Latitude, gp2Latitude), Math.Min(gp1Longitude, gp2Longitude));
            this.topRight = new GeoCoordinate(Math.Max(gp1Latitude, gp2Latitude), Math.Max(gp1Longitude, gp2Longitude));
            this.center = new GeoCoordinate((gp1Latitude + gp2Latitude) / 2, (gp1Longitude + gp2Longitude) / 2);
        }

        public Viewport(GeoCoordinate gp1, GeoCoordinate gp2)
        {
            this.bottomLeft = new GeoCoordinate(Math.Min(gp1.Latitude, gp2.Latitude), Math.Min(gp1.Longitude, gp2.Longitude));
            this.topRight = new GeoCoordinate(Math.Max(gp1.Latitude, gp2.Latitude), Math.Max(gp1.Longitude, gp2.Longitude));
            this.center = new GeoCoordinate((gp1.Latitude + gp2.Latitude) / 2, (gp1.Longitude + gp2.Longitude) / 2);
        }

        /**
         * Check whether a point is contained in this viewport.
         *
         * @param point
         *            the coordinates to check
         * @return true if the point is contained in this viewport, false otherwise or if the point contains no coordinates
         */
        public bool Contains(GeoCoordinate coords) {
            return coords != null
                    && coords.GetLongitudeE6() >= bottomLeft.GetLongitudeE6()
                    && coords.GetLongitudeE6() <= topRight.GetLongitudeE6()
                    && coords.GetLatitudeE6() >= bottomLeft.GetLatitudeE6()
                    && coords.GetLatitudeE6() <= topRight.GetLatitudeE6();
        }

    }
}
