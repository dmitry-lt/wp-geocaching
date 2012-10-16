using System;
using System.Windows.Data;
using WP_Geocaching.Model.Api.GeocachingSu;

namespace WP_Geocaching.Model.Converters
{
    public class IconConverter : IValueConverter
    {
        private const string IconUri = "/Resources/Icons/ic_cache_custom_{0}_{1}.png";
        private const string CheckpointUri = "/Resources/Icons/ic_checkpoint_{0}.png";

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                // TODO: implement icons for opencaching.com
                return new Uri(String.Format(IconUri, "traditional", "valid"), UriKind.Relative);
            }

            Enum[] iconIdentifier = value as Enum[];
            GeocachingSuCache.Types type = (GeocachingSuCache.Types)iconIdentifier[0];
            GeocachingSuCache.Subtypes subtype = (GeocachingSuCache.Subtypes)iconIdentifier[1];
            switch (type)
            {
                case GeocachingSuCache.Types.Traditional:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "traditional", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "traditional", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "traditional", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.StepbyStepTraditional:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "step_by_step_traditional", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "step_by_step_traditional", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "step_by_step_traditional", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Virtual:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "virtual", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "virtual", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "virtual", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Event:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "event", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "event", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "event", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Camera:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "camera", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "camera", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "camera", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Extreme:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "extreme", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "extreme", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "extreme", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.StepbyStepVirtual:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "step_by_step_virtual", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "step_by_step_virtual", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "step_by_step_virtual", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case GeocachingSuCache.Types.Competition:
                    {
                        switch (subtype)
                        {
                            case GeocachingSuCache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "competition", "not_confirmed"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "competition", "not_valid"), UriKind.Relative);
                            case GeocachingSuCache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "competition", "valid"), UriKind.Relative);
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

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
