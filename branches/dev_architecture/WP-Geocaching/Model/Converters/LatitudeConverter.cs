using System;
using System.Windows.Data;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.Model.Converters
{
    public class LatitudeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var latitude = (double)value;

            var directionLatitude = latitude > 0 ? AppResources.NorthLatitude : AppResources.SouthLatitude;
            var degreeLatitude = Math.Abs((int)latitude);
            var minuteLatitude = Math.Abs(latitude - (int)latitude) * 60;

            return String.Format(AppResources.LatitudeFormat, degreeLatitude, minuteLatitude, directionLatitude);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}