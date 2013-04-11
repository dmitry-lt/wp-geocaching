using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Device.Location;
using System.Globalization;

namespace GeocachingPlus.Model.Api.OpencachingDe
{
    public class OpencachingDeCacheParser
    {
        private OpencachingDeCache.Types GetType(string text)
        {
            if (text == null)
            {
                return OpencachingDeCache.Types.Unknown;
            }
            switch (text.ToLower())
            {
                case "2":
                    return OpencachingDeCache.Types.Traditional;
                case "3":
                    return OpencachingDeCache.Types.Multi;
                case "5":
                    return OpencachingDeCache.Types.Webcam;
                case "7":
                    return OpencachingDeCache.Types.Quiz;
                case "6":
                    return OpencachingDeCache.Types.Event;
                case "8":
                    return OpencachingDeCache.Types.Math;
                case "9":
                    return OpencachingDeCache.Types.Moving;
                case "10":
                    return OpencachingDeCache.Types.DriveIn;
                case "4":
                    return OpencachingDeCache.Types.Virtual;
                case "1":
                    return OpencachingDeCache.Types.Unknown;
                default:
                    return OpencachingDeCache.Types.Unknown;
            }
        }

        public List<Cache> Parse(XDocument caches)
        {
            var cacheList = (from c in caches.Descendants("cache")
                      select new OpencachingDeCache()
                      {
                          Id = c.Attribute("wp").Value,
                          Location = new GeoCoordinate(Convert.ToDouble(c.Attribute("lat").Value,
             CultureInfo.InvariantCulture), Convert.ToDouble(c.Attribute("lon").Value,
             CultureInfo.InvariantCulture)),
                          Type = (OpencachingDeCache.Types)GetType(c.Attribute("type").Value),
                      }).Cast<Cache>().ToList();
            return cacheList;
        }

        public SearchResult ParseForException(XDocument doc)
        {
            var searchRes = new SearchResult();
            XElement generalElement = doc
                   .Element("searchresult");
            searchRes.Count = Convert.ToInt32(generalElement.Element("count").Value);
            searchRes.Available = Convert.ToInt32(generalElement.Element("available").Value);
            searchRes.MaxRecord = Convert.ToInt32(generalElement.Element("maxrecordreached").Value);
           
            return searchRes;
        }

        public class SearchResult
        {
            public int Count { get; set; }
            public int Available { get; set; }
            public int MaxRecord { get; set; }
        }

    }
}
