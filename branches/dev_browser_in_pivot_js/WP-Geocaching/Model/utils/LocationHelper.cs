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
using System.Device.Location;

namespace WP_Geocaching.Model.Utils
{
    public class LocationHelper
    {
        public static double CacheAzimuth(GeoCoordinate currentCoordinate, GeoCoordinate soughtCoordinate)
        {
            double y = Math.Sin((soughtCoordinate.Longitude - currentCoordinate.Longitude) * Math.PI / 180) * Math.Cos(soughtCoordinate.Latitude * Math.PI / 180);
            double x = Math.Cos(currentCoordinate.Latitude * Math.PI / 180) * Math.Sin(soughtCoordinate.Latitude * Math.PI / 180) -
                Math.Sin(currentCoordinate.Latitude * Math.PI / 180) * Math.Cos(soughtCoordinate.Latitude * Math.PI / 180) * Math.Cos((soughtCoordinate.Longitude - currentCoordinate.Longitude) * Math.PI / 180);
            double cacheAzimuth = (Math.Atan2(y, x) * 180 / Math.PI + 360) % 360;

            return cacheAzimuth;
        }
    }
}