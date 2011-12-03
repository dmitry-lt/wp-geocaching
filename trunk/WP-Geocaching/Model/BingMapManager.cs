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

namespace WP_Geocaching.Model
{
    /// <summary>
    /// Contains information for BingMap
    /// </summary>
    public static class BingMapManager
    {
        public static GeoCoordinate DefaulMapCenter = new GeoCoordinate(59.879904, 29.828674);
        public static int DefaultZoom = 13;
    }
}
