﻿using System;
using System.Text.RegularExpressions;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class UTFGridPosition
    {
        private const string PATTERN_JSON_KEY = "[^\\d]*" + "(\\d+),\\s*(\\d+)" + "[^\\d]*"; // (12, 34)

        public readonly int x;
        public readonly int y;

        public UTFGridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /**
         * @param key
         *            Key in the format (xx, xx)
         * @return
         */
        public static UTFGridPosition FromString(string key) {
            try
            {
                var match = Regex.Match(key, PATTERN_JSON_KEY);

                var x = Convert.ToInt32(match.Groups[1].Value);
                var y = Convert.ToInt32(match.Groups[2].Value);

                return new UTFGridPosition(x, y);
            } 
            catch (Exception e) 
            {
                return new UTFGridPosition(0, 0);
            }
        }

    }
}
