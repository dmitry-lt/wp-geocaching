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
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            switch ((Cache.Types)value)
            {
                case Cache.Types.Traditional:
                    return CacheTypeResources.traditional;
                case Cache.Types.StepbyStepTraditional:
                    return CacheTypeResources.step_by_step_traditional;
                case Cache.Types.Virtual:
                    return CacheTypeResources.cVirtual;
                case Cache.Types.Event:
                    return CacheTypeResources.cEvent;
                case Cache.Types.Camera:
                    return CacheTypeResources.camera;
                case Cache.Types.Extreme:
                    return CacheTypeResources.extreme;
                case Cache.Types.StepbyStepVirtual:
                    return CacheTypeResources.step_by_step_virtual;
                case Cache.Types.Competition:
                    return CacheTypeResources.competition;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

