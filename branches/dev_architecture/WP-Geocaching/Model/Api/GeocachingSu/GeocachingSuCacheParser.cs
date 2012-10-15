using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Device.Location;
using System.Globalization;

namespace WP_Geocaching.Model.Api.GeocachingSu
{
    /// <summary>
    /// Parses xml for get cache list
    /// </summary>
    public class GeocachingSuCacheParser
    {
        public List<Cache> Parse(XElement caches)
        {
            return caches.Elements("c").Select(p => new Cache
                   {
                       Id = p.Element("id").Value,
                       Location = new GeoCoordinate(Convert.ToDouble(p.Element("la").Value, 
                           CultureInfo.InvariantCulture), Convert.ToDouble(p.Element("ln").Value, 
                           CultureInfo.InvariantCulture)),
                       Name = p.Element("n").Value,
                       Subtype = (Cache.Subtypes)Convert.ToInt32(p.Element("st").Value),
                       Type = (Cache.Types)Convert.ToInt32(p.Element("ct").Value),
                       CClass = getCClassList(p.Element("cc").Value),
                       CacheProvider = CacheProvider.GeocachingSu,
                   }).ToList();
        }

        private List<int> getCClassList(string st)
        {
            var cClass = new List<int>();
            var curCClass = "";
            for (var i = 0; i < st.Length; i++)
            {
                if (st[i] == ',')
                {
                    cClass.Add(Convert.ToInt32(curCClass));
                    curCClass = "";
                }
                else
                {
                    curCClass += st[i];
                    if (i + 1 == st.Length)
                    {
                        cClass.Add(Convert.ToInt32(curCClass));
                    }
                }
            }
            return cClass;
        }
    }
}
