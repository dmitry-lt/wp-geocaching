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
    public class CacheTypeConverter : IValueConverter
    {
        public static string[] types = new string[]{
            null,
            CacheTypeResources.traditional,
            CacheTypeResources.step_by_step_traditional, 
            CacheTypeResources.cVirtual, 
            CacheTypeResources.cEvent, 
            CacheTypeResources.camera, 
            CacheTypeResources.extreme, 
            CacheTypeResources.step_by_step_virtual, 
            CacheTypeResources.competition
        };

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string st = types[System.Convert.ToInt32(value)];
            return st;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

