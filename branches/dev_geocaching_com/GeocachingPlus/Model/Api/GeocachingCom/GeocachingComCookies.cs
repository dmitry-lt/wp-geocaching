using System.Net;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public static class GeocachingComCookies
    {
        private static readonly CookieContainer CookieContainerField = new CookieContainer();

        public static CookieContainer CookieContainer { get { return CookieContainerField; } }
    }
}
