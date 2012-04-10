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
using System.Device.Location;
using System.ComponentModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.ViewModel
{
    public class CachePushpin
    {
        private GeoCoordinate location;
        private string id;
        private Enum[] iconUri;
        private ICommand showDetails;

        public GeoCoordinate Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }
        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public Enum[] IconUri
        {
            get
            {
                return this.iconUri;
            }
            set
            {
                this.iconUri = value;
            }
        }
        public ICommand ShowDetails
        {
            get
            {
                return this.showDetails;
            }
        }

        public CachePushpin()
        {
            showDetails = new ButtonCommand(DefaultShowDetails);
        }

        public CachePushpin(Cache cache)
        {
            Location = cache.Location;
            Id = cache.Id.ToString();
            IconUri = new Enum[2] { cache.Type, cache.Subtype };
            showDetails = new ButtonCommand(DefaultShowDetails);
        }

        public CachePushpin(DbCheckpointsItem item)
        {
            Location = new GeoCoordinate(item.Latitude, item.Longitude);
            Id = "-1";
            IconUri = new Enum[2] { (Cache.Types)item.Type, (Cache.Subtypes)item.Subtype };
        }

        private void DefaultShowDetails(object p)
        {
            NavigationManager.Instance.NavigateToDetails(id);
        }
    }
}
