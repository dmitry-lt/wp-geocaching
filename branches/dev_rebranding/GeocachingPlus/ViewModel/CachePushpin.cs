using System.Windows.Input;
using System.Device.Location;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.Model.Navigation;

namespace GeocachingPlus.ViewModel
{
    public class CachePushpin
    {
        private ICommand showDetails;

        public Cache CacheInfo { get; set; }

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
            CacheInfo = cache;
            showDetails = new ButtonCommand(DefaultShowDetails);
        }

        public CachePushpin(DbCheckpoint item)
        {
            // TODO: refactor
            CacheInfo = new GeocachingSuCache()
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
            NavigationManager.Instance.NavigateToInfoPivot(CacheInfo, isAppBarEnabled);
        }
    }
}
