using System.Windows.Input;
using System.Device.Location;
using WP_Geocaching.Model;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model.Navigation;

namespace WP_Geocaching.ViewModel
{
    public class CachePushpin
    {
        private ICommand showDetails;

        public Cache Cache { get; set; }

        public ICommand ShowDetails
        {
            get
            {
                return showDetails;
            }
        }

        public CachePushpin()
        {
            showDetails = new ButtonCommand(DefaultShowDetails);
        }

        public CachePushpin(Cache cache)
        {
            Cache = cache;
            showDetails = new ButtonCommand(DefaultShowDetails);
        }

        public CachePushpin(DbCheckpoint item)
        {
            // TODO: refactor
            Cache = new GeocachingSuCache()
                        {
                            Location = new GeoCoordinate(item.Latitude, item.Longitude),
                            Id = "-1",
                            Type = (GeocachingSuCache.Types) item.Type,
                            Subtype = (GeocachingSuCache.Subtypes) item.Subtype,
                        };
        }

        private void DefaultShowDetails(object p)
        {
            var isAppBarEnabled = !(p != null && !(bool) p);
            NavigationManager.Instance.NavigateToInfoPivot(Cache, isAppBarEnabled);
        }
    }
}
