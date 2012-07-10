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
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.View.Compass;

namespace WP_Geocaching.ViewModel
{
    public class SearchBingMapViewModel : BaseMapViewModel, ICompassView
    {
        private const int MinLatitude = -90;
        private const int MaxLatitude = 90;
        private const int MinLongitude = -180;
        private const int MaxLongitude = 180;

        private bool isFirstSettingView;
        private GeoCoordinateWatcher watcher;
        private int zoom;
        private ObservableCollection<CachePushpin> cachePushpins;
        private Cache soughtCache;
        private Action<LocationRect> setView;
        private LocationCollection connectingLine;
        private double distanceToSoughtPoint;
        private Settings settings;

        public override int Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                zoom = value;
                NotifyPropertyChanged("Zoom");
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
                    settings.LastSoughtCacheId = value.Id;
                }
            }
        }

        public override ObservableCollection<CachePushpin> CachePushpins
        {
            get
            {
                return this.cachePushpins;
            }
            set
            {
                cachePushpins = value;
                NotifyPropertyChanged("CachePushpins");
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
                connectingLine = value;
                NotifyPropertyChanged("ConnectingLine");
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
                UpdateCurrentLocationInConnectingLine(value);
                UpdateConnectingLineLength();
                currentLocation = value;
                settings.LastLocation = value;
                NotifyPropertyChanged("CurrentLocation");
            }
        }

        public double DistanceToSoughtPoint
        {
            get
            {
                return this.distanceToSoughtPoint;
            }
            set
            {
                distanceToSoughtPoint = value;
                NotifyPropertyChanged("DistanceToSoughtPoint");
            }
        }

        public SearchBingMapViewModel(IApiManager apiManager, Action<LocationRect> setView)
        {
            this.apiManager = apiManager;
            this.setView = setView;
            settings = new Settings();
            isFirstSettingView = true;
            Zoom = MapManager.Instance.DefaultZoom;
            CachePushpins = new ObservableCollection<CachePushpin>();
            ConnectingLine = new LocationCollection();
            currentLocation = settings.LastLocation;
            ShowAll();

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 20;
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(PositionChanged);
            watcher.Start();
        }

        public void UpdateMapChildrens()
        {
            UpdateCachePushpins();
            UpdateConnectingLine();
            UpdateConnectingLineLength();
        }

        public void ShowAll()
        {
            var northwest = new GeoCoordinate(MinLatitude, MaxLongitude);
            var southeast = new GeoCoordinate(MaxLatitude, MinLongitude);
            foreach (CachePushpin c in cachePushpins)
            {
                UpdateToSetData(c.Location, northwest, southeast);
            }
            UpdateToSetData(CurrentLocation, northwest, southeast);
            setView(new LocationRect(northwest.Latitude, northwest.Longitude, southeast.Latitude, southeast.Longitude));
        }

        public void SetDirection(double direction)
        {   
            double northDirection = -direction;
            Direction = (360 - northDirection) % 360;
        }

        private void UpdateToSetData(GeoCoordinate coordinate, GeoCoordinate northwest, GeoCoordinate southeast)
        {
            northwest.Latitude = Math.Max(coordinate.Latitude, northwest.Latitude);
            northwest.Longitude = Math.Min(coordinate.Longitude, northwest.Longitude);
            southeast.Latitude = Math.Min(coordinate.Latitude, southeast.Latitude);
            southeast.Longitude = Math.Max(coordinate.Longitude, southeast.Longitude);
        }

        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            CurrentLocation = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

            if ((isFirstSettingView) && (CachePushpins.Count != 0))
            {
                ShowAll();
                isFirstSettingView = false;
            }
        }

        private void UpdateCurrentLocationInConnectingLine(GeoCoordinate newCurrentLocation)
        {
            ConnectingLine.Remove(currentLocation);
            ConnectingLine.Add(newCurrentLocation);
        }

        private void UpdateCachePushpins()
        {
            var db = new CacheDataBase();
            var dbCheckpointsList = db.GetCheckpointsbyCacheId(MapManager.Instance.CacheId);
            var cachePushpins = new ObservableCollection<CachePushpin>();
            cachePushpins.Add(new CachePushpin(SoughtCache));
            foreach (DbCheckpointsItem c in dbCheckpointsList)
            {
                cachePushpins.Add(new CachePushpin(c));
            }
            CachePushpins = cachePushpins;
        }

        private void UpdateConnectingLine()
        {
            var connectingLine = new LocationCollection();
            if (currentLocation != null)
            {
                connectingLine.Add(currentLocation);
            }
            connectingLine.Add(GetSoughtPoint());
            ConnectingLine = connectingLine;
        }

        private void UpdateConnectingLineLength()
        {
            if (ConnectingLine.Count == 2)
            {
                DistanceToSoughtPoint = ConnectingLine[0].GetDistanceTo(ConnectingLine[1]);
            }
        }

        private GeoCoordinate GetSoughtPoint()
        {
            foreach (CachePushpin c in CachePushpins)
            {
                var subtype = (Cache.Subtypes)c.IconUri[1];
                if ((subtype == Cache.Subtypes.ActiveCheckpoint))
                {
                    return c.Location;
                }
            }
            return SoughtCache.Location;
        }
    }
}
