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

namespace WP_Geocaching.Model.Converters
{
    public class IconConverter : IValueConverter
    {
        private const string IconUri = "/Resources/Icons/ic_cache_custom_{0}_{1}.png";
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
            if (value == null)
            {
                return null;
            }

            string st = value.ToString();
            if (st.Equals("arrow"))
            {
                return new Uri("/Resources/Icons/ic_arrow.png", UriKind.Relative);
            }

            String sUri = String.Format(IconUri, types[System.Convert.ToInt32(st[0].ToString())],
                subtypes[System.Convert.ToInt32(st[1].ToString())]);
            Uri convertedUri = new Uri(sUri, UriKind.Relative);
            return convertedUri;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
