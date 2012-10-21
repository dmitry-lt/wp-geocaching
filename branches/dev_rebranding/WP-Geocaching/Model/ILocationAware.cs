using System.Device.Location;

namespace GeocachingPlus.Model
{
    public interface ILocationAware
    {
        bool IsNeedHighAccuracy { get; set; }

        void ProcessLocation(GeoCoordinate location);
    }
}
