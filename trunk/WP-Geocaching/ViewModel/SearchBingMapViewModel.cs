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

        private Boolean isFirstSettingView;
        private GeoCoordinate northwest;
        private GeoCoordinate southeast;
        private GeoCoordinateWatcher watcher;
        private int zoom;
        private GeoCoordinate mapCenter;
        private IApiManager apiManager;
        private ObservableCollection<CachePushpin> cachePushpins;
        private Cache soughtCache;
        private Action<LocationRect> setView;
        private LocationCollection connectingLine;
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
        public Cache SoughtCache
        {
            get
            {
                return this.soughtCache;
            }
            set
            {
                soughtCache = value;               
                if (soughtCache != null)
                {
                    MapManager.Instance.CacheId = value.Id;
                    ConnectingLine.Add(soughtCache.Location);
                    UpdateMapChildrens();
                }
            }
        }
        public ObservableCollection<CachePushpin> CachePushpins
        {
            get
            {
                return this.cachePushpins;
            }
            set
            {
                bool changed = cachePushpins != value;
                if (changed)
                {
                    cachePushpins = value;
                    OnPropertyChanged("CachePushpins");
                }
            }
        }
        public LocationCollection ConnectingLine
        {
            get
            {
                return this.connectingLine;
            }
            set
            {
                bool changed = connectingLine != value;
                if (changed)
                {
                    connectingLine = value;
                    OnPropertyChanged("ConnectingLine");
                }
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
                    UpdateCurrentLocationinConectingLine(value);
                    currentLocation = value;
                    OnPropertyChanged("CurrentLocation");
                }
            }
        }

        public SearchBingMapViewModel(IApiManager apiManager, Action<LocationRect> setView)
        {
            this.apiManager = apiManager;
            this.setView = setView;

            isFirstSettingView = true;
            northwest = new GeoCoordinate(-90, 180);
            southeast = new GeoCoordinate(90, -180);
            Zoom = MapManager.Instance.DefaultZoom;            
            CachePushpins = new ObservableCollection<CachePushpin>();
            ConnectingLine = new LocationCollection();

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 20;
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            watcher.Start();
        }
        
        public void UpdateMapChildrens()
        {
            UpdateCachePushpins();
            UpdateConnectingLine();
        }

        public void SetViewAll()
        {
            GeoCoordinate northwest = new GeoCoordinate(-90, 180);
            GeoCoordinate southeast = new GeoCoordinate(90, -180);
            foreach (CachePushpin c in cachePushpins)
            {
                UpdateToSetData(c.Location, northwest, southeast);
            }
            UpdateToSetData(CurrentLocation, northwest, southeast);
            setView(new LocationRect(northwest.Latitude, northwest.Longitude, southeast.Latitude, southeast.Longitude));
        }

        private void UpdateToSetData(GeoCoordinate coordinate, GeoCoordinate northwest, GeoCoordinate southeast)
        {
            northwest.Latitude = Math.Max(coordinate.Latitude, northwest.Latitude);
            northwest.Longitude = Math.Min(coordinate.Longitude, northwest.Longitude);
            southeast.Latitude = Math.Min(coordinate.Latitude, southeast.Latitude);
            southeast.Longitude = Math.Max(coordinate.Longitude, southeast.Longitude);          
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            CurrentLocation = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

            if ((isFirstSettingView) && (CachePushpins.Count != 0))
            {
                SetViewAll();
                isFirstSettingView = false;
            }
        }

        private void UpdateCurrentLocationinConectingLine(GeoCoordinate newCurrentLocation)
        {
            ConnectingLine.Remove(currentLocation);
            ConnectingLine.Add(newCurrentLocation);
        }             

        private void UpdateCachePushpins()
        {
            CacheDataBase db = new CacheDataBase();
            List<DbCheckpointsItem> dbCheckpointsList = db.GetCheckpointsbyCacheId(MapManager.Instance.CacheId);
            ObservableCollection<CachePushpin> cachePushpins = new ObservableCollection<CachePushpin>();
            cachePushpins.Add(new CachePushpin(SoughtCache));
            foreach (DbCheckpointsItem c in dbCheckpointsList)
            {
                cachePushpins.Add(new CachePushpin(c));
            }
            CachePushpins = cachePushpins;
        }

        private void UpdateConnectingLine()
        {
            LocationCollection connectingLine = new LocationCollection();
            if (currentLocation != null)
            {
                connectingLine.Add(currentLocation);
            }
            connectingLine.Add(GetSoughtPoint());
            ConnectingLine = connectingLine;
        }

        private GeoCoordinate GetSoughtPoint()
        {
            foreach (CachePushpin c in CachePushpins)
            {
                Cache.Subtypes subtype = (Cache.Subtypes)c.IconUri[1];
                if ((subtype == Cache.Subtypes.ActiveCheckpoint))
                {
                    return c.Location;
                }
            }
            return SoughtCache.Location;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
