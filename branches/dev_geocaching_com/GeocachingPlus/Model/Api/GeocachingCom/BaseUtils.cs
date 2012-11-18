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
        public static bool Matches(string data, string pattern) {
            if (data == null) {
                return false;
            }
            // matcher is faster than String.contains() and more flexible - it takes patterns instead of fixed texts
            return Regex.IsMatch(data, pattern, RegexOptions.Singleline);
        }
    }
}
