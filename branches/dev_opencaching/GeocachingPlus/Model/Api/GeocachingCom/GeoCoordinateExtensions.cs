using System;
using System.Device.Location;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public static class GeoCoordinateExtensions
    {
        public static int GetLongitudeE6(this GeoCoordinate coordinate)
        {
            return (int)Math.Round(coordinate.Longitude * 1E6);
        }

        public static int GetLatitudeE6(this GeoCoordinate coordinate)
        {
            return (int)Math.Round(coordinate.Latitude * 1E6);
        }
    }
}
