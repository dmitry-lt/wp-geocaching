using System;
using System.Windows.Data;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.ViewModel;

namespace WP_Geocaching.Model.Converters
{
    public class IconConverter : IValueConverter
    {
        private const string GeocachingSuIconUri = "/Resources/Icons/GeocachingSu/ic_cache_custom_{0}_{1}.png";
        private const string CheckpointUri = "/Resources/Icons/ic_checkpoint_{0}.png";

        private object ConvertGeocachingSu(GeocachingSuCache.Types type, GeocachingSuCache.Subtypes subtype)
        {
            switch (type)
            {
                case GeocachingSuCache.Types.Traditional:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(GeocachingSuIconUri, "traditional", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(GeocachingSuIconUri, "traditional", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(GeocachingSuIconUri, "traditional", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.StepbyStepTraditional:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(GeocachingSuIconUri, "step_by_step_traditional", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(GeocachingSuIconUri, "step_by_step_traditional", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(GeocachingSuIconUri, "step_by_step_traditional", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Virtual:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(GeocachingSuIconUri, "virtual", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(GeocachingSuIconUri, "virtual", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(GeocachingSuIconUri, "virtual", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Event:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(GeocachingSuIconUri, "event", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(GeocachingSuIconUri, "event", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(GeocachingSuIconUri, "event", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Camera:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(GeocachingSuIconUri, "camera", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(GeocachingSuIconUri, "camera", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(GeocachingSuIconUri, "camera", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Extreme:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(GeocachingSuIconUri, "extreme", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(GeocachingSuIconUri, "extreme", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(GeocachingSuIconUri, "extreme", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.StepbyStepVirtual:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(GeocachingSuIconUri, "step_by_step_virtual", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(GeocachingSuIconUri, "step_by_step_virtual", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(GeocachingSuIconUri, "step_by_step_virtual", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Competition:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(GeocachingSuIconUri, "competition", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(GeocachingSuIconUri, "competition", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(GeocachingSuIconUri, "competition", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Checkpoint:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.ActiveCheckpoint:
                                return new Uri(String.Format(CheckpointUri, "active"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotActiveCheckpoint:
                                return new Uri(String.Format(CheckpointUri, "not_active"), UriKind.Relative);
                        }
                        break;
                    }
            }

            return null;

        }

        private object ConvertGeocachingSu(GeocachingSuCache value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ConvertGeocachingSu(value.Type, value.Subtype);
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                // TODO: implement icons for opencaching.com
                return new Uri(String.Format(GeocachingSuIconUri, "traditional", "valid"), UriKind.Relative);
            }

            if (value is GeocachingSuCache)
            {
                return ConvertGeocachingSu(value as GeocachingSuCache, targetType, parameter, culture);
            }

            // TODO: refactor
            if (value is ListCacheItem)
            {
                var lci = value as ListCacheItem;
                return ConvertGeocachingSu((GeocachingSuCache.Types)lci.Type, (GeocachingSuCache.Subtypes)lci.Subtype);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
