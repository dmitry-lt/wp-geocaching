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

namespace WP_Geocaching.ViewModel
{
    public class CachePushpin
    {
        private GeoCoordinate location;
        private string cacheId;
        private Uri iconUri;

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
        public string CacheId
        {
            get
            {
                return this.cacheId;
            }
            set
            {
                this.cacheId = value;
            }
        }
        public Uri IconUri
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

        public CachePushpin()
        {
        }
    }
}
