﻿using System.ComponentModel;
using GeocachingPlus.Model.Utils;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class GeocachingComCache : Cache, INotifyPropertyChanged
    {
        public enum Types
        {
            TRADITIONAL = 201,
            MULTI = 202,
            MYSTERY = 203,
            LETTERBOX = 204,
            EVENT = 205,
            MEGA_EVENT = 206,
            EARTH = 207,
            CITO = 208,
            WEBCAM = 209,
            VIRTUAL = 210,
            WHERIGO = 211,
            LOSTANDFOUND = 212,
            PROJECT_APE = 213,
            GCHQ = 214,
            GPS_EXHIBIT = 215,
            UNKNOWN = 216,
        }

        private Types _type;
        public Types Type
        {
            get { return _type; }
            set 
            { 
                _type = value;
                PropertyChanged.Raise(() => Type);
            }
        }

        public bool ReliableLocation { get; set; }

        public GeocachingComCache()
        {
            CacheProvider = CacheProvider.GeocachingCom;
            Type = Types.UNKNOWN;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
