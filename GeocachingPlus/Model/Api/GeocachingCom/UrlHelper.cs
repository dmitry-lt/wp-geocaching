using System.Collections.Generic;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class UrlHelper
    {
        public static string FormUrl(string baseUrl, Dictionary<string, string> parameters)
        {
            var urlString = baseUrl;
            var firstParameter = true;
            foreach (var k in parameters.Keys)
            {
                if (firstParameter)
                {
                    urlString += "?" + k + "=" + parameters[k];
                    firstParameter = false;
                }
                else
                {
                    urlString += "&" + k + "=" + parameters[k];
                }
            }
            return urlString;
        }
    }
}
