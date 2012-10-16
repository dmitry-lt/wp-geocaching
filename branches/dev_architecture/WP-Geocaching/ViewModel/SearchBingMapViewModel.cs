﻿using System;
using WP_Geocaching.Model;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.View.Compass;

namespace WP_Geocaching.ViewModel
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

        public override int Zoom
        {
            get
            {
                return zoom;
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
                return soughtCache;
            }
            set
            {
                soughtCache = value;
                if (soughtCache != null)
                {
                    MapManager.Instance.CacheId = value.Id;
                    ConnectingLine.Add(soughtCache.Location);
                    UpdateMapProperties();
                    settings.LastSoughtCacheId = value.Id;
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
                NotifyPropertyChanged("CachePushpins");
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
                NotifyPropertyChanged("ConnectingLine");
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
                settings.LastLocation = value;
                NotifyPropertyChanged("CurrentLocation");
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
                NotifyPropertyChanged("DistanceToSoughtPoint");
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
            settings = new Settings();
            MapMode = settings.MapMode;
            currentLocation = settings.LastLocation;
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
                UpdateToSetData(c.Cache.Location, northwest, southeast);
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
            var dbCheckpointsList = db.GetCheckpointsByCacheId(MapManager.Instance.CacheId);
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
            foreach (var c in CachePushpins)
            {
                // TODO: refactor
                if (c.Cache is GeocachingSuCache)
                {
                    var subtype = (c.Cache as GeocachingSuCache).Subtype;
                    if ((subtype == GeocachingSuCache.Subtypes.ActiveCheckpoint))
                    {
                        return c.Cache.Location;
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