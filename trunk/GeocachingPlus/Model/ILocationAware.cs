using System.Device.Location;

namespace GeocachingPlus.Model
{
    public interface ILocationAware
    {
        bool IsNeedHighAccuracy { get; }

        void ProcessLocation(GeoCoordinate location);
    }
}
