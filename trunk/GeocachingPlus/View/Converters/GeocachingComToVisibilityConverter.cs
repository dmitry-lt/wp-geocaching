using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GeocachingPlus.Model.Api.GeocachingCom;

namespace GeocachingPlus.View.Converters
{
    public class GeocachingComToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = value is GeocachingComCache;
            if ("reverse".Equals(parameter))
            {
                visible = !visible;
            }
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
