using System;
using GeocachingPlus.Model;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;
using GeocachingPlus.Model.Api;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.View.Compass;

namespace GeocachingPlus.ViewModel
{
    public class SearchBingMapViewModel : BaseMapViewModel, ICompassAware, ILocationAware
    {
        private const int MinLatitude = -90;
        private const int MaxLatitude = 90;
        private const int MinLongitude = -180;
        private const int MaxLongitude = 180;

        private bool isFirstSettingView;
        private int zoom;
        private ObservableCollection<CachePushpin> cachePushpins;
        private Cache soughtCache;
        private Action<LocationRect> setView;
        private LocationCollection connectingLine;
        private double distanceToSoughtPoint;

        private GeoCoordinate mapCenter;
        public override GeoCoordinate MapCenter
        {
            get
            {
                return mapCenter;
            }
            set
            {
                mapCenter = value;
                RaisePropertyChanged(() => MapCenter);
            }
        }

        public override int Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = value;
                RaisePropertyChanged(() => Zoom);
            }
        }

        public Cache SoughtCache
        {
            get
            {
                return soughtCache;
            }
            set
            {
                soughtCache = value;
                if (soughtCache != null)
                {
                    MapManager.Instance.Cache = value;
                    ConnectingLine.Add(soughtCache.Location);
                    UpdateMapProperties();
                    var settings = new Settings();
                    settings.LatestSoughtCacheId = value.Id;
                    settings.LatestSoughtCacheProvider = value.CacheProvider;
                }
            }
        }

        public override ObservableCollection<CachePushpin> CachePushpins
        {
            get
            {
                return cachePushpins;
            }
            set
            {
                cachePushpins = value;
                RaisePropertyChanged(() => CachePushpins);
            }
        }

        public LocationCollection ConnectingLine
        {
            get
            {
                return connectingLine;
            }
            set
            {
                connectingLine = value;
                RaisePropertyChanged(() => ConnectingLine);
            }
        }

        public GeoCoordinate CurrentLocation
        {
            get
            {
                return currentLocation;
            }
            set
            {
                UpdateCurrentLocationInConnectingLine(value);
                UpdateConnectingLineLength();
                currentLocation = value;
                var settings = new Settings();
                settings.LatestSearchLocation = value;
                RaisePropertyChanged(() => CurrentLocation);
            }
        }

        public double DistanceToSoughtPoint
        {
            get
            {
                return distanceToSoughtPoint;
            }
            set
            {
                distanceToSoughtPoint = value;
                RaisePropertyChanged(() => DistanceToSoughtPoint);
            }
        }

        public bool IsNeedHighAccuracy
        {
            get
            {
                return true;
            }
            set { }
        }

        public SearchBingMapViewModel(IApiManager apiManager, Action<LocationRect> setView)
        {
            this.apiManager = apiManager;
            this.setView = setView;
            isFirstSettingView = true;
            Zoom = MapManager.Instance.DefaultZoom;
            CachePushpins = new ObservableCollection<CachePushpin>();
            ConnectingLine = new LocationCollection();
            var settings = new Settings();
            MapMode = settings.MapMode;
            currentLocation = settings.LatestSearchLocation;
            ShowAll();
        }

        public override void UpdateMapProperties()
        {
            UpdateCachePushpins();
            UpdateConnectingLine();
            UpdateConnectingLineLength();
            UpdateMapMode();
        }

        public void ShowAll()
        {
            var northwest = new GeoCoordinate(MinLatitude, MaxLongitude);
            var southeast = new GeoCoordinate(MaxLatitude, MinLongitude);
            foreach (var c in cachePushpins)
            {
                UpdateToSetData(c.CacheInfo.Location, northwest, southeast);
            }
            UpdateToSetData(CurrentLocation, northwest, southeast);
            setView(new LocationRect(northwest.Latitude, northwest.Longitude, southeast.Latitude, southeast.Longitude));
        }

        public void SetDirection(double direction)
        {   
            var northDirection = -direction;
            Direction = (360 - northDirection) % 360;
        }

        private void UpdateToSetData(GeoCoordinate coordinate, GeoCoordinate northwest, GeoCoordinate southeast)
        {
            northwest.Latitude = Math.Max(coordinate.Latitude, northwest.Latitude);
            northwest.Longitude = Math.Min(coordinate.Longitude, northwest.Longitude);
            southeast.Latitude = Math.Min(coordinate.Latitude, southeast.Latitude);
            southeast.Longitude = Math.Max(coordinate.Longitude, southeast.Longitude);
        }

        private void UpdateCurrentLocationInConnectingLine(GeoCoordinate newCurrentLocation)
        {
            ConnectingLine.Remove(currentLocation);
            ConnectingLine.Add(newCurrentLocation);
        }

        private void UpdateCachePushpins()
        {
            var db = new CacheDataBase();
            var dbCheckpointsList = db.GetCheckpointsByCache(MapManager.Instance.Cache);
            var cachePushpins = new ObservableCollection<CachePushpin>();
            cachePushpins.Add(new CachePushpin(SoughtCache));
            foreach (DbCheckpoint c in dbCheckpointsList)
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
            foreach (var c in CachePushpins)
            {
                // TODO: refactor
                if (c.CacheInfo is GeocachingSuCache)
                {
                    var subtype = (c.CacheInfo as GeocachingSuCache).Subtype;
                    if ((subtype == GeocachingSuCache.Subtypes.ActiveCheckpoint))
                    {
                        return c.CacheInfo.Location;
                    }
                }
            }
            return SoughtCache.Location;
        }

        public void ProcessLocation(GeoCoordinate location)
        {
            CurrentLocation = location;

            if ((isFirstSettingView) && (CachePushpins.Count != 0))
            {
                ShowAll();
                isFirstSettingView = false;
            }
        }
    }
}
