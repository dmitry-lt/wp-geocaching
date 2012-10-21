using System;
using System.Windows.Data;
using WP_Geocaching.Resources.Localization;
using System.Device.Location;

namespace WP_Geocaching.View.Converters
{
    public class CoordinateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var coordinate = (GeoCoordinate)value;

            var directionLatitude = coordinate.Latitude > 0 ? AppResources.NorthLatitude : AppResources.SouthLatitude;
            var degreeLatitude = Math.Abs((int)coordinate.Latitude);
            var minuteLatitude = Math.Abs(coordinate.Latitude - (int)coordinate.Latitude) * 60;

            var directionLongitude = coordinate.Longitude > 0 ? AppResources.EastLongitude : AppResources.WestLongitude;
            var degreeLongitude = Math.Abs((int)coordinate.Longitude);
            var minuteLongitude = Math.Abs(coordinate.Longitude - (int)coordinate.Longitude) * 60;

            return String.Format(AppResources.CoordinateFormat, degreeLatitude, minuteLatitude, directionLatitude, degreeLongitude, minuteLongitude, directionLongitude);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}