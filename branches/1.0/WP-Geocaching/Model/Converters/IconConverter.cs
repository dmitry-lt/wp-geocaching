using System;
using System.Windows.Data;

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
                return null;
            }

            Enum[] iconIdentifier = value as Enum[];
            Cache.Types type = (Cache.Types)iconIdentifier[0];
            Cache.Subtypes subtype = (Cache.Subtypes)iconIdentifier[1];
            switch (type)
            {
                case Cache.Types.Traditional:
                    {
                        switch (subtype)
                        {
                            case Cache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "traditional", "not_confirmed"), UriKind.Relative);
                            case Cache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "traditional", "not_valid"), UriKind.Relative);
                            case Cache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "traditional", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case Cache.Types.StepbyStepTraditional:
                    {
                        switch (subtype)
                        {
                            case Cache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "step_by_step_traditional", "not_confirmed"), UriKind.Relative);
                            case Cache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "step_by_step_traditional", "not_valid"), UriKind.Relative);
                            case Cache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "step_by_step_traditional", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case Cache.Types.Virtual:
                    {
                        switch (subtype)
                        {
                            case Cache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "virtual", "not_confirmed"), UriKind.Relative);
                            case Cache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "virtual", "not_valid"), UriKind.Relative);
                            case Cache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "virtual", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case Cache.Types.Event:
                    {
                        switch (subtype)
                        {
                            case Cache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "event", "not_confirmed"), UriKind.Relative);
                            case Cache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "event", "not_valid"), UriKind.Relative);
                            case Cache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "event", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case Cache.Types.Camera:
                    {
                        switch (subtype)
                        {
                            case Cache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "camera", "not_confirmed"), UriKind.Relative);
                            case Cache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "camera", "not_valid"), UriKind.Relative);
                            case Cache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "camera", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case Cache.Types.Extreme:
                    {
                        switch (subtype)
                        {
                            case Cache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "extreme", "not_confirmed"), UriKind.Relative);
                            case Cache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "extreme", "not_valid"), UriKind.Relative);
                            case Cache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "extreme", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case Cache.Types.StepbyStepVirtual:
                    {
                        switch (subtype)
                        {
                            case Cache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "step_by_step_virtual", "not_confirmed"), UriKind.Relative);
                            case Cache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "step_by_step_virtual", "not_valid"), UriKind.Relative);
                            case Cache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "step_by_step_virtual", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case Cache.Types.Competition:
                    {
                        switch (subtype)
                        {
                            case Cache.Subtypes.NotConfirmed:
                                return new Uri(String.Format(IconUri, "competition", "not_confirmed"), UriKind.Relative);
                            case Cache.Subtypes.NotValid:
                                return new Uri(String.Format(IconUri, "competition", "not_valid"), UriKind.Relative);
                            case Cache.Subtypes.Valid:
                                return new Uri(String.Format(IconUri, "competition", "valid"), UriKind.Relative);
                        }
                        break;
                    }
                case Cache.Types.Checkpoint:
                    {
                        switch (subtype)
                        {
                            case Cache.Subtypes.ActiveCheckpoint:
                                return new Uri(String.Format(CheckpointUri, "active"), UriKind.Relative);
                            case Cache.Subtypes.NotActiveCheckpoint:
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
