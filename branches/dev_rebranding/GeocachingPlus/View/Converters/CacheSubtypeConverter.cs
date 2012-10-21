using System;
using System.Windows.Data;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Resources.Localization;

namespace GeocachingPlus.View.Converters
{
    public class CacheSubtypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            switch ((GeocachingSuCache.Subtypes)value)
            {
                case GeocachingSuCache.Subtypes.NotConfirmed:
                    return CacheSubtypeResources.not_confirmed;
                case GeocachingSuCache.Subtypes.NotValid:
                    return CacheSubtypeResources.not_valid;
                case GeocachingSuCache.Subtypes.Valid:
                    return CacheSubtypeResources.valid;
                case GeocachingSuCache.Subtypes.ActiveCheckpoint:
                    return CacheSubtypeResources.active;
                case GeocachingSuCache.Subtypes.NotActiveCheckpoint:
                    return CacheSubtypeResources.not_active;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
