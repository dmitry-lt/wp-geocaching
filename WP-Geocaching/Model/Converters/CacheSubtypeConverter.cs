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
    public class CacheSubtypeConverter : IValueConverter
    {
        public static string[] subtypes = new string[]{
            null, 
            CacheSubtypeResources.valid, 
            CacheSubtypeResources.not_confirmed, 
            CacheSubtypeResources.not_valid
        };

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string st = subtypes[System.Convert.ToInt32(value)];
            return st;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
