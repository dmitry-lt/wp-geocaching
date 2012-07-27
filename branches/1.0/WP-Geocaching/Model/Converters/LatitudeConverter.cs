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
    public class LatitudeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            double latitude = (double)value;

            string directionLatitude = latitude > 0 ? AppResources.NorthLatitude : AppResources.SouthLatitude;
            int degreeLatitude = Math.Abs((int)latitude);
            double minuteLatitude = Math.Abs(latitude - (int)latitude) * 60;

            return String.Format(AppResources.LatitudeFormat, degreeLatitude, minuteLatitude, directionLatitude);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}