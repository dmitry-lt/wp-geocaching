using System;
using System.Windows.Data;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.View.Converters
{
    public class LongitudeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var longitude = (double)value;

            var directionLongitude = longitude > 0 ? AppResources.EastLongitude : AppResources.WestLongitude;
            var degreeLongitude = Math.Abs((int)longitude);
            var minuteLongitude = Math.Abs(longitude - (int)longitude) * 60;

            return String.Format(AppResources.LongitudeFormat, degreeLongitude, minuteLongitude, directionLongitude);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}