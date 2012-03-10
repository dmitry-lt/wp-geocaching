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
                                IconUri = new Uri(cache.Type.ToString() + cache.Subtype.ToString(), UriKind.Relative),
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
                       
            CachePushpin pin = new CachePushpin();
            pin.CacheId = "-1";
            pin.IconUri = new Uri("arrow", UriKind.Relative);
            pin.Location = manager.DefaulMapCenter;
            CachePushpinCollection.Add(pin);
            Locations.Add(pin.Location);

            this.watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            this.watcher.MovementThreshold = 20;
            this.watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            this.watcher.Start();

        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {         
            CachePushpin pin = new CachePushpin();
            pin.CacheId = "-1";
            pin.IconUri = new Uri("arrow", UriKind.Relative);
            pin.Location = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
            CachePushpinCollection.RemoveAt(0);
            CachePushpinCollection.Insert(0, pin);

            Locations.RemoveAt(0);
            Locations.Insert(0, pin.Location);

            if ((isFirst) && (cachePushpinCollection.Count == 2))
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
                    if (!pin.Equals(cachePushpinCollection[0]))
                    {
                        refreshToSetData(pin.Location, northwest, southeast);
                    }
                }
            }           
        }

        public void SetViewAll()
        {
            GeoCoordinate northwest = new GeoCoordinate(this.northwest.Latitude, this.northwest.Longitude);
            GeoCoordinate southeast = new GeoCoordinate(this.southeast.Latitude, this.southeast.Longitude);
            refreshToSetData(cachePushpinCollection[0].Location, northwest, southeast);
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
