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
using System.Collections.Generic;
using System.Xml.Linq;

namespace WP_Geocaching.Model
{
    public class IconConverter : IValueConverter
    {
        public static string[] types = new string[]{
            "",
            "traditional",
            "step_by_step_traditional", 
            "virtual", 
            "event", 
            "camera", 
            "extreme", 
            "step_by_step_virtual", 
            "competition"
        };

        /// <summary>
        /// Values of Cache.Subtype
        /// </summary>
        public static string[] subtypes = new string[]{
            "", 
            "valid", 
            "not_confirmed", 
            "not_valid"
        };

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            string st = value.ToString();
            Uri convertedUri = new Uri("/Resources/Icons/ic_cache_custom_" + types[System.Convert.ToInt32(st[0].ToString())] +
                "_" + subtypes[System.Convert.ToInt32(st[1].ToString())] + ".png", UriKind.Relative);
            return convertedUri;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
