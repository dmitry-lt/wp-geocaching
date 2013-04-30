using System;
using System.Device.Location;
using GeocachingPlus.Model.Api;

namespace GeocachingPlus.Model
{
    public class Cluster : Cache
    {
        public Cluster()
        {
            CacheProvider = CacheProvider.Cluster;    
        }
    }
}
