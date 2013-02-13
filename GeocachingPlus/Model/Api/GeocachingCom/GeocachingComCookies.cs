using System.Net;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public static class GeocachingComCookies
    {
        private static CookieContainer _cookieContainerField;

        static GeocachingComCookies()
        {
            ResetCookies();
        }

        public static void ResetCookies()
        {
            _cookieContainerField = new CookieContainer();
        }

        public static CookieContainer CookieContainer { get { return _cookieContainerField; } }
    }
}
