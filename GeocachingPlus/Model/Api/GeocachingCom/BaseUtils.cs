using System.Text.RegularExpressions;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class BaseUtils
    {
        /**
         * Searches for the pattern p in the data.
         *
         * @return true if data contains the pattern p
         */
        public static bool Matches(string data, string pattern)
        {
            var matches = Regex.Matches(data, pattern, RegexOptions.Singleline);
            if (matches.Count == 1)
            {
                var value = matches[0].Groups[0].Value;
                return !string.IsNullOrWhiteSpace(value);
            }

            return false;
        }
    }
}
