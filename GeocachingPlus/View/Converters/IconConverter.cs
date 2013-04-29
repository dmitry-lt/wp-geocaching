using System;
using System.Windows.Data;
using GeocachingPlus.Model.Api.GeocachingCom;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Model.Api.OpenCachingCom;
using GeocachingPlus.Model.Api.OpencachingDe;
using GeocachingPlus.ViewModel;

namespace GeocachingPlus.View.Converters
{
    public class IconConverter : IValueConverter
    {
        private const string GeocachingSuIconUri = "/Resources/Icons/GeocachingSu/ic_cache_custom_{0}_{1}.png";
        private const string CheckpointUri = "/Resources/Icons/ic_checkpoint_{0}.png";

        private const string OpenCachingComIconUri = "/Resources/Icons/OpenCachingCom/ic_cache_custom_{0}_valid.png";

        private const string OpencachingDeIconUri = "/Resources/Icons/OpencachingDe/ic_cache_{0}.png";

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

        private object ConvertGeocachingSu(GeocachingSuCache value)
        {
            return ConvertGeocachingSu(value.Type, value.Subtype);
        }

        private object ConvertOpenCachingCom(OpenCachingComCache cache)
        {
            switch (cache.Type)
            {
                case OpenCachingComCache.Types.Traditional:
                    return new Uri(String.Format(OpenCachingComIconUri, "traditional"), UriKind.Relative);

                case OpenCachingComCache.Types.Multi:
                    return new Uri(String.Format(OpenCachingComIconUri, "multi"), UriKind.Relative);

                case OpenCachingComCache.Types.Puzzle:
                    return new Uri(String.Format(OpenCachingComIconUri, "puzzle"), UriKind.Relative);

                case OpenCachingComCache.Types.Virtual:
                    return new Uri(String.Format(OpenCachingComIconUri, "virtual"), UriKind.Relative);

            }
            return null;
        }

        private object ConvertOpencachingDe(OpencachingDeCache cache)
        {
            switch (cache.Type)
            {
                case OpencachingDeCache.Types.Traditional:
                    return new Uri(String.Format(OpencachingDeIconUri, "traditional"), UriKind.Relative);

                case OpencachingDeCache.Types.Multi:
                    return new Uri(String.Format(OpencachingDeIconUri, "multi"), UriKind.Relative);

                case OpencachingDeCache.Types.DriveIn:
                    return new Uri(String.Format(OpencachingDeIconUri, "drive-in"), UriKind.Relative);

                case OpencachingDeCache.Types.Event:
                    return new Uri(String.Format(OpencachingDeIconUri, "event"), UriKind.Relative);
                
                case OpencachingDeCache.Types.Math:
                    return new Uri(String.Format(OpencachingDeIconUri, "math"), UriKind.Relative);
                
                case OpencachingDeCache.Types.Moving:
                    return new Uri(String.Format(OpencachingDeIconUri, "traditional"), UriKind.Relative); // another icon
                
                case OpencachingDeCache.Types.Quiz:
                    return new Uri(String.Format(OpencachingDeIconUri, "quiz"), UriKind.Relative);

                case OpencachingDeCache.Types.Virtual:
                    return new Uri(String.Format(OpencachingDeIconUri, "virtual"), UriKind.Relative);

                case OpencachingDeCache.Types.Webcam:
                    return new Uri(String.Format(OpencachingDeIconUri, "webcam"), UriKind.Relative);

                case OpencachingDeCache.Types.Unknown:
                    return new Uri(String.Format(OpencachingDeIconUri, "unknown"), UriKind.Relative);
            }
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is OpenCachingComCache)
            {
                return ConvertOpenCachingCom(value as OpenCachingComCache);
            }

            if (value is GeocachingSuCache)
            {
                return ConvertGeocachingSu(value as GeocachingSuCache);
            }

            if (value is GeocachingComCache)
            {
                // TODO: implement
                return new Uri(String.Format(CheckpointUri, "not_active"), UriKind.Relative);
            }

            if (value is OpencachingDeCache)
            {
                return ConvertOpencachingDe(value as OpencachingDeCache);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
