using System;
using System.Globalization;
using System.Windows.Data;
using GeocachingPlus.Model.Api.GeocachingCom;

namespace GeocachingPlus.View.Converters
{
    public class GeocachingComIconConverter : IValueConverter
    {
        private const string GeocachingComIconUri = "/Resources/Icons/GeocachingCom/type_{0}.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GeocachingComCache)
            {
                var cache = value as GeocachingComCache;

                switch (cache.Type)
                {
                    case GeocachingComCache.Types.TRADITIONAL:
                        return new Uri(String.Format(GeocachingComIconUri, "traditional"), UriKind.Relative);

                    case GeocachingComCache.Types.MULTI:
                        return new Uri(String.Format(GeocachingComIconUri, "multi"), UriKind.Relative);

                    case GeocachingComCache.Types.MYSTERY:
                        return new Uri(String.Format(GeocachingComIconUri, "mystery"), UriKind.Relative);

                    case GeocachingComCache.Types.LETTERBOX:
                        return new Uri(String.Format(GeocachingComIconUri, "letterbox"), UriKind.Relative);

                    case GeocachingComCache.Types.EVENT:
                        return new Uri(String.Format(GeocachingComIconUri, "event"), UriKind.Relative);

                    case GeocachingComCache.Types.MEGA_EVENT:
                        return new Uri(String.Format(GeocachingComIconUri, "mega"), UriKind.Relative);

                    case GeocachingComCache.Types.EARTH:
                        return new Uri(String.Format(GeocachingComIconUri, "earth"), UriKind.Relative);

                    case GeocachingComCache.Types.CITO:
                        return new Uri(String.Format(GeocachingComIconUri, "cito"), UriKind.Relative);

                    case GeocachingComCache.Types.WEBCAM:
                        return new Uri(String.Format(GeocachingComIconUri, "webcam"), UriKind.Relative);

                    case GeocachingComCache.Types.VIRTUAL:
                        return new Uri(String.Format(GeocachingComIconUri, "virtual"), UriKind.Relative);

                    case GeocachingComCache.Types.WHERIGO:
                        return new Uri(String.Format(GeocachingComIconUri, "wherigo"), UriKind.Relative);

                    case GeocachingComCache.Types.LOSTANDFOUND:
                        return new Uri(String.Format(GeocachingComIconUri, "event"), UriKind.Relative);

                    case GeocachingComCache.Types.PROJECT_APE:
                        return new Uri(String.Format(GeocachingComIconUri, "ape"), UriKind.Relative);

                    case GeocachingComCache.Types.GCHQ:
                        return new Uri(String.Format(GeocachingComIconUri, "hq"), UriKind.Relative);

                    case GeocachingComCache.Types.GPS_EXHIBIT:
                        return new Uri(String.Format(GeocachingComIconUri, "traditional"), UriKind.Relative);

                    case GeocachingComCache.Types.UNKNOWN:
                        return new Uri(String.Format(GeocachingComIconUri, "unknown"), UriKind.Relative);

                }

            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
