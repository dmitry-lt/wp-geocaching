﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WP_Geocaching.Model;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Device.Location;

namespace WP_Geocaching.Model
{
    /// <summary>
    /// Parses xml for get cache list
    /// </summary>
    public class CacheParser
    {
        public List<Cache> Parse(XElement caches)
        {
            List<Cache> cacheList = new List<Cache>();
            foreach (XElement p in caches.Elements("c"))
            {
                Cache cache = new Cache();
                cache.Id = Convert.ToInt32(p.Element("id").Value);
                cache.Location = new GeoCoordinate(Convert.ToDouble(p.Element("la").Value), Convert.ToDouble(p.Element("ln").Value));
                cache.Name = p.Element("n").Value;
                cache.Subtype = Convert.ToInt32(p.Element("st").Value);
                cache.Type = Convert.ToInt32(p.Element("ct").Value);
                cache.CClass = this.getCClassList(p.Element("cc").Value);
                cacheList.Add(cache);
            }
            return cacheList;
        }
        private List<int> getCClassList(string st)
        {
            List<int> cClass = new List<int>();
            string curCClass = "";
            for (int i = 0; i < st.Length; i++)
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
