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

namespace GeocachingPlus.Model.Api.OpencachingDe
{
    public class OpencachingDeCache : Cache
    {
        public enum Types
        {
            Traditional = 301,
            Multi = 302,
            Webcam = 303,
            Event = 304,
            Quiz = 305,
            Math = 306,
            Moving = 307,
            DriveIn = 308,
            Virtual = 309,
            Unknown = 310
        }

        public enum Sizes
        {
            Normal = 1,
            Small = 2,
            No_container = 3,
            Micro = 4,
            Large = 5
        }

        public OpencachingDeCache()
        {
            CacheProvider = CacheProvider.OpenCachingCom;
        }

        public Types Type { get; set; }
        public Sizes Size { get; set; }

    }
}
