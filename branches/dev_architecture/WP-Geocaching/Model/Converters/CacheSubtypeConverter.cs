﻿using System;
using System.Windows.Data;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.Model.Converters
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

            switch ((Cache.Subtypes)value)
            {
                case Cache.Subtypes.NotConfirmed:
                    return CacheSubtypeResources.not_confirmed;
                case Cache.Subtypes.NotValid:
                    return CacheSubtypeResources.not_valid;
                case Cache.Subtypes.Valid:
                    return CacheSubtypeResources.valid;
                case Cache.Subtypes.ActiveCheckpoint:
                    return CacheSubtypeResources.active;
                case Cache.Subtypes.NotActiveCheckpoint:
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
