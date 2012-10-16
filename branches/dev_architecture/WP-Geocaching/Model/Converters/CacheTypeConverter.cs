using System;
using System.Windows.Data;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.Model.Converters
{
    public class CacheTypeConverter : IValueConverter
    {      
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is GeocachingSuCache)
            {
                switch ((value as GeocachingSuCache).Type)
                {
                    case GeocachingSuCache.Types.Traditional:
                        return CacheTypeResources.traditional;
                    case GeocachingSuCache.Types.StepbyStepTraditional:
                        return CacheTypeResources.step_by_step_traditional;
                    case GeocachingSuCache.Types.Virtual:
                        return CacheTypeResources.cVirtual;
                    case GeocachingSuCache.Types.Event:
                        return CacheTypeResources.cEvent;
                    case GeocachingSuCache.Types.Camera:
                        return CacheTypeResources.camera;
                    case GeocachingSuCache.Types.Extreme:
                        return CacheTypeResources.extreme;
                    case GeocachingSuCache.Types.StepbyStepVirtual:
                        return CacheTypeResources.step_by_step_virtual;
                    case GeocachingSuCache.Types.Competition:
                        return CacheTypeResources.competition;
                    case GeocachingSuCache.Types.Checkpoint:
                        return CacheTypeResources.checkpoint;
                    default:
                        return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

