using System.Collections.Generic;
using System.Net;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class UrlHelper
    {
        public static string FormUrl(string baseUrl, Dictionary<string, string> parameters)
        {
            return baseUrl + "?" + FormUrlParameterQuery(parameters);
        }

        public static string FormUrlParameterQuery(Dictionary<string, string> parameters)
        {
            var result = "";
            var firstParameter = true;
            foreach (var k in parameters.Keys)
            {
                if (!firstParameter)
                {
                    result += "&";
                }
                result += k + "=" + parameters[k];
                firstParameter = false;
            }
            return result;
        }
    }
}
