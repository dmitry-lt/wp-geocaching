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

namespace WP_Geocaching.Model
{
    public class CachePushpin
    {
        private GeoCoordinate location;
        private string name;
        private Uri iconUri;
        private EventHandler<GestureEventArgs> tap;

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
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public EventHandler<GestureEventArgs> Tap
        {
            get
            {
                return this.tap;
            }
            set
            {
                this.tap = value;
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
            this.Tap = this.ShowDetails;
        }

        /// <summary>
        /// ToDo: naviggates to cache details page
        /// </summary>
        private void ShowDetails(object sender, GestureEventArgs e)
        {
        }
    }
}
