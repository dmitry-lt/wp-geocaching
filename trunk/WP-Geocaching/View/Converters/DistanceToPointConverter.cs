using System;
using System.Windows.Data;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.View.Converters
{
    public class DistanceToPointConverter : IValueConverter
    {
        private const string DistanceToPoint = "≈ {0} {1}";

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var distantion = (double)value;

            string formatedDistace;
            if (distantion >= 1000)
            {
                distantion = Math.Round(distantion / 100) / 10;
                formatedDistace = String.Format(DistanceToPoint, distantion, AppResources.Kilometres);
            }
            else
            {
                distantion = Math.Round(distantion * 10) / 10;
                formatedDistace = String.Format(DistanceToPoint, distantion, AppResources.Metres);
            }
            return formatedDistace;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
