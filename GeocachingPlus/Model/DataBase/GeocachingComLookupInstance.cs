using System.Device.Location;
using GeocachingPlus.Model.Api.GeocachingCom;

namespace GeocachingPlus.Model.DataBase
{
    public class GeocachingComLookupInstance
    {
        public string Id;
        public GeocachingComCache.Types Type;
        public GeoCoordinate Location;
        public bool ReliableLocation;
    }
}