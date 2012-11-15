using System;
using System.Windows.Data;
using GeocachingPlus.Model.Api.GeocachingCom;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Model.Api.OpenCachingCom;
using GeocachingPlus.Resources.Localization;

namespace GeocachingPlus.View.Converters
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

            if (value is OpenCachingComCache)
            {
                switch ((value as OpenCachingComCache).Type)
                {
                    case OpenCachingComCache.Types.Traditional:
                        return OpenCachingComCacheType.Traditional;
                    case OpenCachingComCache.Types.Multi:
                        return OpenCachingComCacheType.Multi;
                    case OpenCachingComCache.Types.Virtual:
                        return OpenCachingComCacheType.Virtual;
                    case OpenCachingComCache.Types.Puzzle:
                        return OpenCachingComCacheType.Puzzle;
                    default:
                        return null;
                }
            }

            if (value is GeocachingComCache)
            {
                switch ((value as GeocachingComCache).Type)
                {
                    case GeocachingComCache.Types.TRADITIONAL:
                        return GeocachingComCacheType.TRADITIONAL;

                    case GeocachingComCache.Types.MULTI:
                        return GeocachingComCacheType.MULTI;

                    case GeocachingComCache.Types.MYSTERY:
                        return GeocachingComCacheType.MYSTERY;

                    case GeocachingComCache.Types.LETTERBOX:
                        return GeocachingComCacheType.LETTERBOX;

                    case GeocachingComCache.Types.EVENT:
                        return GeocachingComCacheType.EVENT;

                    case GeocachingComCache.Types.MEGA_EVENT:
                        return GeocachingComCacheType.MEGA_EVENT;

                    case GeocachingComCache.Types.EARTH:
                        return GeocachingComCacheType.EARTH;

                    case GeocachingComCache.Types.CITO:
                        return GeocachingComCacheType.CITO;

                    case GeocachingComCache.Types.WEBCAM:
                        return GeocachingComCacheType.WEBCAM;

                    case GeocachingComCache.Types.VIRTUAL:
                        return GeocachingComCacheType.VIRTUAL;

                    case GeocachingComCache.Types.WHERIGO:
                        return GeocachingComCacheType.WHERIGO;

                    case GeocachingComCache.Types.LOSTANDFOUND:
                        return GeocachingComCacheType.LOSTANDFOUND;

                    case GeocachingComCache.Types.PROJECT_APE:
                        return GeocachingComCacheType.PROJECT_APE;

                    case GeocachingComCache.Types.GCHQ:
                        return GeocachingComCacheType.GCHQ;

                    case GeocachingComCache.Types.GPS_EXHIBIT:
                        return GeocachingComCacheType.GPS_EXHIBIT;

                    case GeocachingComCache.Types.UNKNOWN:
                        return GeocachingComCacheType.UNKNOWN;

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

