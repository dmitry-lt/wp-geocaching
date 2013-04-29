using System;
using System.Text;

namespace GeocachingPlus.Model.Api
{
    public class WebBrowserHelper
    {
        // International UTF-8 Characters in Windows Phone 7 WebBrowser Control
        // See http://matthiasshapiro.com/2010/10/25/international-utf-8-characters-in-windows-phone-7-webbrowser-control/
        public static string ConvertExtendedASCII(string html)
        {
            var s = html.ToCharArray();

            var sb = new StringBuilder();

            foreach (var c in s)
            {
                var intValue = Convert.ToInt32(c);
                if (intValue > 127)
                    sb.AppendFormat("&#{0};", intValue);
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }


    }
}
