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

namespace WP_Geocaching.Model
{
    public static class CacheManager
    {
        /// <summary>
        /// Values of Cache.Type
        /// </summary>
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
    }
}
