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
using System.Windows.Threading;
using System.Threading;
using System.Xml;
using System.Text;
using System.IO;
using System.Xml.Linq;
using WP_Geocaching.Model;
using System.ComponentModel;
using System.Collections.Generic;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;

namespace WP_Geocaching.ViewModel
{
    public class BingMapViewModel : BaseMapViewModel
    {

        private const int maxCacheCount = 50;
        private IApiManager apiManager;
        private LocationRect boundingRectangle;
        private Visibility surpassedCacheCountMessageVisibility = Visibility.Collapsed;
        private GeoCoordinateWatcher watcher;
        private bool isFirstSettingView;

        public int Zoom { get; set; }
        public ObservableCollection<CachePushpin> CachePushpins { get; set; }

        public Visibility SurpassedCacheCountMessageVisibility
        {
            get
            {
                return this.surpassedCacheCountMessageVisibility;
            }
            set
            {
                surpassedCacheCountMessageVisibility = value;
                NotifyPropertyChanged("SurpassedCacheCountMessageVisibility");
            }
        }

        public LocationRect BoundingRectangle
        {
            get
            {
                return this.boundingRectangle;
            }
            set
            {
                this.boundingRectangle = value;
                SetPushpinsOnMap();
            }
        }

        public BingMapViewModel(IApiManager apiManager)
        {
            var settings = new Settings();
            MapCenter = settings.LastLocation;
            Zoom = MapManager.Instance.DefaultZoom;
            this.apiManager = apiManager;
            CachePushpins = new ObservableCollection<CachePushpin>();
            isFirstSettingView = true;

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.MovementThreshold = 20;
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(PositionChanged);
            watcher.Start();
        }

        private void ProcessCacheList(List<Cache> caches)
        {

            this.CachePushpins.Clear();

            if (caches.Count >= maxCacheCount)
            {

                if (surpassedCacheCountMessageVisibility.Equals(Visibility.Collapsed))
                {
                    SurpassedCacheCountMessageVisibility = Visibility.Visible;
                }
            }
            else
            {

                foreach (Cache p in caches)
                {
                    var pushpin = new CachePushpin()
                    {
                        Location = p.Location,
                        Id = p.Id.ToString(),
                        IconUri = new Enum[2] { p.Type, p.Subtype }
                    };

                    CachePushpins.Add(pushpin);
                }

                if (surpassedCacheCountMessageVisibility.Equals(Visibility.Visible))
                {
                    SurpassedCacheCountMessageVisibility = Visibility.Collapsed;
                }
            }
        }

        private void SetPushpinsOnMap()
        {
            this.apiManager.UpdateCacheList(ProcessCacheList, BoundingRectangle.East,
                BoundingRectangle.West, BoundingRectangle.North, BoundingRectangle.South);
        }

        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var settings = new Settings();
            var currentLocation = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
            settings.LastLocation = currentLocation;
            this.currentLocation = currentLocation;
            if (isFirstSettingView)
            {
                MapCenter = currentLocation;
                NotifyPropertyChanged("MapCenter");
                isFirstSettingView = false;
            }
        }
    }
}
