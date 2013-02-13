using System;
using System.Device.Location;
using System.Text.RegularExpressions;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    /**
 * Parse coordinates.
 */
    public class GeopointParser
    {
        private class ResultWrapper
        {
            public double result;
            public int matcherPos;
            public int matcherLength;

            public ResultWrapper(double result, int matcherPos, int stringLength)
            {
                this.result = result;
                this.matcherPos = matcherPos;
                this.matcherLength = stringLength;
            }
        }

        //                                    ( 1  )    ( 2  )         ( 3  )       ( 4  )       (        5        )
        private const string patternLat = "\\b([NS])\\s*(\\d+)°?(?:\\s*(\\d+)(?:[.,](\\d+)|'?\\s*(\\d+(?:[.,]\\d+)?)(?:''|\")?)?)?";
        private const string patternLon = "\\b([WE])\\s*(\\d+)°?(?:\\s*(\\d+)(?:[.,](\\d+)|'?\\s*(\\d+(?:[.,]\\d+)?)(?:''|\")?)?)?";

        enum LatLon
        {
            LAT,
            LON
        }

        /**
         * Parses a pair of coordinates (latitude and longitude) out of a String.
         * Accepts following formats and combinations of it:
         * X DD
         * X DD°
         * X DD° MM
         * X DD° MM.MMM
         * X DD° MM SS
         *
         * as well as:
         * DD.DDDDDDD
         *
         * Both . and , are accepted, also variable count of spaces (also 0)
         *
         * @param text
         *            the string to parse
         * @return an Geopoint with parsed latitude and longitude
         * @throws Geopoint.ParseException
         *             if lat or lon could not be parsed
         */
        public static GeoCoordinate Parse(String text)
        {
            ResultWrapper latitudeWrapper = ParseHelper(text, LatLon.LAT);
            double lat = latitudeWrapper.result;
            // cut away the latitude part when parsing the longitude
            ResultWrapper longitudeWrapper = ParseHelper(text.Substring(latitudeWrapper.matcherPos + latitudeWrapper.matcherLength), LatLon.LON);

            if (longitudeWrapper.matcherPos - (latitudeWrapper.matcherPos + latitudeWrapper.matcherLength) >= 10)
            {
                return null;
            }

            double lon = longitudeWrapper.result;
            return new GeoCoordinate(lat, lon);
        }

        /**
         * Parses a pair of coordinates (latitude and longitude) out of a String.
         * Accepts following formats and combinations of it:
         * X DD
         * X DD°
         * X DD° MM
         * X DD° MM.MMM
         * X DD° MM SS
         *
         * as well as:
         * DD.DDDDDDD
         *
         * Both . and , are accepted, also variable count of spaces (also 0)
         *
         * @param latitude
         *            the latitude string to parse
         * @param longitude
         *            the longitude string to parse
         * @return an Geopoint with parsed latitude and longitude
         * @throws Geopoint.ParseException
         *             if lat or lon could not be parsed
         */
        public static GeoCoordinate Parse(String latitude, String longitude)
        {
            double lat = ParseLatitude(latitude);
            double lon = ParseLongitude(longitude);

            return new GeoCoordinate(lat, lon);
        }

        /*
         * (non JavaDoc)
         * Helper for coordinates-parsing.
         */
        private static ResultWrapper ParseHelper(String text, LatLon latlon)
        {

            string pattern = LatLon.LAT == latlon ? patternLat : patternLon;
            var matches = Regex.Matches(text, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            if (matches.Count > 0)
            {
                var signString = matches[0].Groups[1].Value.ToUpper();
                double sign = signString == "S" || signString == "W" ? -1.0 : 1.0;
                double degree = Convert.ToInt32(matches[0].Groups[2].Value);

                double minutes = 0.0;
                double seconds = 0.0;

                if (!String.IsNullOrWhiteSpace(matches[0].Groups[3].Value))
                {
                    minutes = Convert.ToInt32(matches[0].Groups[3].Value);

                    if (!String.IsNullOrWhiteSpace(matches[0].Groups[4].Value))
                    {
                        seconds = Convert.ToDouble("0." + matches[0].Groups[4].Value) * 60.0;
                    }
                    if (!String.IsNullOrWhiteSpace(matches[0].Groups[5].Value))
                    {
                        seconds = Convert.ToDouble(matches[0].Groups[5].Value.Replace(",", "."));
                    }
                }

                return new ResultWrapper(sign * (degree + minutes / 60.0 + seconds / 3600.0),
                    text.IndexOf(matches[0].Groups[0].Value, StringComparison.Ordinal),
                    matches[0].Groups[0].Value.Length);

            }

            // Nothing found with "N 52...", try to match string as decimaldegree
            try
            {
                String[] items = text.Trim().Split(new char[] { ' ', '\t', '\r', '\n' });
                if (items.Length > 0)
                {
                    int index = (latlon == LatLon.LON ? items.Length - 1 : 0);
                    int pos = (latlon == LatLon.LON ? text.LastIndexOf(items[index], StringComparison.Ordinal) : text.IndexOf(items[index], StringComparison.Ordinal));
                    return new ResultWrapper(Convert.ToDouble(items[index]), pos, items[index].Length);
                }
            }
            catch (Exception e)
            {
                // The right exception will be raised below.
            }

            return null;
        }

        /**
         * Parses latitude out of a given string.
         *
         * @see #parse(String)
         * @param text
         *            the string to be parsed
         * @return the latitude as decimal degree
         * @throws Geopoint.ParseException
         *             if latitude could not be parsed
         */
        public static double ParseLatitude(String text)
        {
            return ParseHelper(text, LatLon.LAT).result;
        }

        /**
         * Parses longitude out of a given string.
         *
         * @see #parse(String)
         * @param text
         *            the string to be parsed
         * @return the longitude as decimal degree
         * @throws Geopoint.ParseException
         *             if longitude could not be parsed
         */
        public static double ParseLongitude(String text)
        {
            return ParseHelper(text, LatLon.LON).result;
        }
    }

}
