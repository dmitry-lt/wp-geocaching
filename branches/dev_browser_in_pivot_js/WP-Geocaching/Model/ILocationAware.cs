using System.Device.Location;

namespace WP_Geocaching.Model
{
    public interface ILocationAware
    {
        bool IsNeedHighAccuracy { get; set; }

        void ProcessLocation(GeoCoordinate location);
    }
}
