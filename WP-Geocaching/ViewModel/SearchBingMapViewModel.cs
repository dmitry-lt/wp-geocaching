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
using WP_Geocaching.Model.DataBase;


namespace WP_Geocaching.ViewModel
{
    public class SearchBingMapViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Boolean isFirst;
        private GeoCoordinate northwest;
        private GeoCoordinate southeast;
        private GeoCoordinateWatcher watcher;
        private int zoom;
        private GeoCoordinate mapCenter;
        private IApiManager apiManager;
        private ObservableCollection<CachePushpin> cachePushpinCollection;
        private Cache cache;
        private Action<LocationRect> setView;
        private LocationCollection locations;
        private GeoCoordinate currentLocation;

        public int Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                bool changed = zoom != value;
                if (changed)
                {
                    zoom = value;
                    OnPropertyChanged("Zoom");
                }
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
                    OnPropertyChanged("MapCenter");
                }
            }
        }
        public Cache Cache
        {
            get
            {
                return this.cache;
            }
            set
            {
                cache = value;               
                if (cache != null)
                {
                    MapManager.Instance.CacheId = value.Id;
                    Locations.Add(cache.Location);
                    UpdateCachePushpins();
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
                bool changed = cachePushpinCollection != value;
                if (changed)
                {
                    cachePushpinCollection = value;
                    OnPropertyChanged("CachePushpinCollection");
                }
            }
        }
        public LocationCollection Locations
        {
            get
            {
                return this.locations;
            }
            set
            {
                this.locations = value;
            }
        }
        public GeoCoordinate CurrentLocation
        {
            get
            {
                return this.currentLocation;
            }
            set
            {
                bool changed = currentLocation != value;
                if (changed)
                {
                    Locations.Remove(currentLocation);
                    Locations.Add(value);
                    currentLocation = value;
                    OnPropertyChanged("CurrentLocation");
                }
            }
        }

        public SearchBingMapViewModel(IApiManager apiManager, Action<LocationRect> setView)
        {
            this.apiManager = apiManager;
            this.setView = setView;

            this.isFirst = true;
            this.northwest = new GeoCoordinate(-90, 180);
            this.southeast = new GeoCoordinate(90, -180);
            this.zoom = MapManager.Instance.DefaultZoom;            
            this.CachePushpinCollection = new ObservableCollection<CachePushpin>();
            
            this.Locations = new LocationCollection();

            this.watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            this.watcher.MovementThreshold = 20;
            this.watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            this.watcher.Start();

        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            this.CurrentLocation = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

            if ((isFirst) && (cachePushpinCollection.Count != 0))
            {
                SetViewAll();
                isFirst = false;
            }
        }

        public void UpdateCachePushpins()
        {
            CacheDataBase db = new CacheDataBase();
            List<DbCheckpointsItem> dbCheckpointsList = db.GetCheckpointsbyCacheId(MapManager.Instance.CacheId);
            ObservableCollection<CachePushpin> cachePushpins = new ObservableCollection<CachePushpin>();
            CachePushpin pushpin = new CachePushpin()
            {
                Location = cache.Location,
                CacheId = cache.Id.ToString(),
                IconUri = new Enum[2] { cache.Type, cache.Subtype }
            };
            cachePushpins.Add(pushpin);
            foreach (DbCheckpointsItem c in dbCheckpointsList)
            {
                CachePushpin pin = new CachePushpin()
                {
                    Location = new GeoCoordinate(c.Latitude, c.Longitude),
                    CacheId = "-1",
                    IconUri = new Enum[2] { (Cache.Types)c.Type, (Cache.Subtypes)c.Subtype }
                };
                cachePushpins.Add(pin);
            }
            CachePushpinCollection = cachePushpins;
        }

        public void SetViewAll()
        {
            GeoCoordinate northwest = new GeoCoordinate(-90, 180);
            GeoCoordinate southeast = new GeoCoordinate(90, -180);
            foreach (CachePushpin c in cachePushpinCollection)
            {
                refreshToSetData(c.Location, northwest, southeast);
            }
            refreshToSetData(CurrentLocation, northwest, southeast);
            this.setView(new LocationRect(northwest.Latitude, northwest.Longitude, southeast.Latitude, southeast.Longitude));
        }

        private void refreshToSetData(GeoCoordinate coordinate, GeoCoordinate northwest, GeoCoordinate southeast)
        {
            northwest.Latitude = Math.Max(coordinate.Latitude, northwest.Latitude);
            northwest.Longitude = Math.Min(coordinate.Longitude, northwest.Longitude);
            southeast.Latitude = Math.Min(coordinate.Latitude, southeast.Latitude);
            southeast.Longitude = Math.Max(coordinate.Longitude, southeast.Longitude);          
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
