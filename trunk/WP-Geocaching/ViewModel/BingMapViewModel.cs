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
    public class BingMapViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int zoom;
        private const int maxCountOfCache = 50;
        private GeoCoordinate mapCenter;
        private IApiManager apiManager;
        private ObservableCollection<CachePushpin> cachePushpinCollection;
        private LocationRect boundingRectangle;
        private String surpassedCacheCountMessageVisibility = "Collapsed";
        private String undetectedLocationMessageVisibility = "Collapsed";
        private GeoCoordinateWatcher watcher;
        private bool isFirstSettingView;
        private GeoCoordinate currentLocation;

        public BingMapViewModel(IApiManager apiManager)
        {
            Settings settings = new Settings();
            MapCenter = settings.LastLocation;
            Zoom = MapManager.Instance.DefaultZoom;
            this.apiManager = apiManager;
            CachePushpinCollection = new ObservableCollection<CachePushpin>();
            isFirstSettingView = true;

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 20;
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(PositionChanged);
            watcher.Start();
        }

        public String SurpassedCacheCountMessageVisibility
        {
            get
            {
                return this.surpassedCacheCountMessageVisibility;
            }
        }

        public String UndetectedLocationMessageVisibility
        {
            get
            {
                return this.undetectedLocationMessageVisibility;
            }
            set
            {
                bool changed = undetectedLocationMessageVisibility != value;
                if (changed)
                {
                    undetectedLocationMessageVisibility = value;
                    NotifyPropertyChanged("UndetectedLocationMessageVisibility");
                }
            }
        }

        public int Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                this.zoom = value;
            }
        }

        public GeoCoordinate MapCenter
        {
            get
            {
                return this.mapCenter;
            }
            set
            {
                bool changed = mapCenter != value;
                if (changed)
                {
                    mapCenter = value;
                    NotifyPropertyChanged("MapCenter");
                }
            }
        }

        public ObservableCollection<CachePushpin> CachePushpinCollection
        {
            get
            {
                return this.cachePushpinCollection;
            }
            set
            {
                this.cachePushpinCollection = value;
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
                this.GetPushpins();
            }
        }

        private void ProcessCacheList(List<Cache> caches)
        {

            this.CachePushpinCollection.Clear();

            if (caches.Count >= maxCountOfCache)
            {

                if (surpassedCacheCountMessageVisibility.Equals("Collapsed"))
                {
                    surpassedCacheCountMessageVisibility = "Visible";
                    NotifyPropertyChanged("SurpassedCacheCountMessageVisibility");
                }
            }
            else
            {

                foreach (Cache p in caches)
                {
                    CachePushpin pushpin = new CachePushpin()
                    {
                        Location = p.Location,
                        Id = p.Id.ToString(),
                        IconUri = new Enum[2] { p.Type, p.Subtype }
                    };

                    this.CachePushpinCollection.Add(pushpin);
                }

                if (surpassedCacheCountMessageVisibility.Equals("Visible"))
                {
                    surpassedCacheCountMessageVisibility = "Collapsed";
                    NotifyPropertyChanged("SurpassedCacheCountMessageVisibility");
                }
            }

        }

        private void GetPushpins()
        {
            this.apiManager.GetCacheList(ProcessCacheList, BoundingRectangle.East,
                BoundingRectangle.West, BoundingRectangle.North, BoundingRectangle.South);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Settings settings = new Settings();
            GeoCoordinate currentLocation = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
            settings.LastLocation = currentLocation;
            this.currentLocation = currentLocation;
            if (isFirstSettingView)
            {
                MapCenter = currentLocation;
                NotifyPropertyChanged("MapCenter");
                isFirstSettingView = false;
            }
        }

        public void SetMapCenterOnCurrentLocationOrShowMessage(System.Windows.Threading.Dispatcher dispatcher)
        {
            if (currentLocation == null)
            {
                UndetectedLocationMessageVisibility = "Visible";
                System.Threading.Timer timer = new System.Threading.Timer((state) =>
                {
                    System.Threading.Timer t = (System.Threading.Timer)state;
                    dispatcher.BeginInvoke(() =>
                    {
                        UndetectedLocationMessageVisibility = "Collapsed";
                    });
                    t.Dispose();
                });
                timer.Change(3000, 0);
            }
            else
            {
                MapCenter = currentLocation;
            }
        }
    }
}
