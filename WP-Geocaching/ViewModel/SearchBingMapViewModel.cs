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
    public class SearchBingMapViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Boolean isFirst;
        private GeoCoordinate northwest;
        private GeoCoordinate southeast;
        private GeoCoordinateWatcher watcher;
        private BingMapManager manager;
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
                    CachePushpin pushpin = new CachePushpin()
                    {
                        Location = cache.Location,
                        CacheId = cache.Id.ToString(),
                        IconUri = new Enum[2] { cache.Type, cache.Subtype }
                    };
                    CachePushpinCollection.Add(pushpin);
                    Locations.Add(pushpin.Location);
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
            this.manager = new BingMapManager();
            this.apiManager = apiManager;
            this.setView = setView;

            this.isFirst = true;
            this.northwest = new GeoCoordinate(-90, 180);
            this.southeast = new GeoCoordinate(90, -180);            
            this.zoom = manager.DefaultZoom;            
            this.CachePushpinCollection = new ObservableCollection<CachePushpin>();
            this.Locations = new LocationCollection();
            this.CachePushpinCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(collectionChanged);

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

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {           
            if (e.NewItems != null)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    CachePushpin pin = e.NewItems[i] as CachePushpin;
                    refreshToSetData(pin.Location, northwest, southeast);
                }
            }           
        }

        public void SetViewAll()
        {
            GeoCoordinate northwest = new GeoCoordinate(this.northwest.Latitude, this.northwest.Longitude);
            GeoCoordinate southeast = new GeoCoordinate(this.southeast.Latitude, this.southeast.Longitude);
            refreshToSetData(CurrentLocation, northwest, southeast);
            this.setView(new LocationRect(northwest.Latitude, northwest.Longitude, southeast.Latitude, southeast.Longitude));
        }

        private void refreshToSetData(GeoCoordinate coordinate, GeoCoordinate northwest, GeoCoordinate southeast)
        {
            if (coordinate.Latitude > northwest.Latitude) northwest.Latitude = coordinate.Latitude;
            if (coordinate.Longitude < northwest.Longitude) northwest.Longitude = coordinate.Longitude;
            if (coordinate.Latitude < southeast.Latitude) southeast.Latitude = coordinate.Latitude;
            if (coordinate.Longitude > southeast.Longitude) southeast.Longitude = coordinate.Longitude;          
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
