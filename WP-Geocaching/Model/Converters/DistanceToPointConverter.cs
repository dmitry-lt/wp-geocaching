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

namespace WP_Geocaching.Model.Converters
{
    public class DistanceToPointConverter : IValueConverter
    {
        private const string IconUri = "≈ {0} {1}";

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            double distantion = (double)value;

            if (distantion >= 1000)
            {
                distantion = Math.Round(distantion / 100) / 10;
                return String.Format(IconUri, distantion, AppResources.Kilometres);
            }
            else
            {
                distantion = Math.Round(distantion * 10) / 10;
                return String.Format(IconUri, distantion, AppResources.Metres);
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
