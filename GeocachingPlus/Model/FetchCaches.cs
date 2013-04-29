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

namespace GeocachingPlus.Model
{
    public class FetchCaches
    {
        public List<Cache> Caches { get; set; }
        public bool TooManyCaches { get; set; }

        public FetchCaches(List<Cache> caches, bool tooManyCaches)
        {
            Caches = caches;
            TooManyCaches = tooManyCaches;
        }

    }
}
