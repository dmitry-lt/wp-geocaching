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
using System.Windows.Data;
using WP_Geocaching.Resources.Localization;
using System.Device.Location;

namespace WP_Geocaching.Model.Converters
{
    public class CoordinateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            GeoCoordinate coordinate = (GeoCoordinate)value;

            string directionLatitude = coordinate.Latitude > 0 ? AppResources.NorthLatitude : AppResources.SouthLatitude;
            int degreeLatitude = Math.Abs((int)coordinate.Latitude);
            double minuteLatitude = Math.Abs(coordinate.Latitude - (int)coordinate.Latitude) * 60;

            string directionLongitude = coordinate.Longitude > 0 ? AppResources.EastLongitude : AppResources.WestLongitude;
            int degreeLongitude = Math.Abs((int)coordinate.Longitude);
            double minuteLongitude = Math.Abs(coordinate.Longitude - (int)coordinate.Longitude) * 60;

            return String.Format(AppResources.CoordinateFormat, degreeLatitude, minuteLatitude, directionLatitude, degreeLongitude, minuteLongitude, directionLongitude);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}