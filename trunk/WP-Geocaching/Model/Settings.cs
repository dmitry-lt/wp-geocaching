﻿using System;
using System.IO.IsolatedStorage;
using System.Device.Location;
using WP_Geocaching.Model.Api;

namespace WP_Geocaching.Model
{
    public enum MapMode
    {
        Road = 0,
        Aerial = 1
    }

    public class Settings
    {
        private IsolatedStorageSettings settings;

        // The isolated storage key names of our settings
        private const string LastSoughtCacheIdKeyName = "LastSoughtCacheId";
        private const string LastSoughtCacheProviderKeyName = "LastSoughtCacheProvider";
        private const string LastLocationLatitudeDefaultKeyName = "LastLocationLatitude";
        private const string LastLocationLongitudeDefaultKeyName = "LastLocationLongitude";
        private const string MapModeDefaultKeyName = "MapMode";
        private const string IsLocationEnabledKeyName = "IsLocationEnabled";
        private const string IsFirstLaunchingKeyName = "IsFirstLaunching";

        // The default value of our settings
        private const string LastSoughtCacheIdDefault = "";
        private const double LastLocationLatitudeDefault = 59.879904;
        private const double LastLocationLongitudeDefault = 29.828674;
        private const int MapModeDefault = (int)MapMode.Road;
        private const bool IsLocationEnabledDefault = true;
        private const bool IsFirstLaunchingDefault = true;

        public Settings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;

            if (settings.Contains(key))
            {
                if (settings[key] != value)
                {
                    settings[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                settings.Add(key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            if (settings.Contains(key))
            {
                value = (T)settings[key];
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }

        public void Save()
        {
            settings.Save();
        }


        public string LastSoughtCacheId
        {
            get
            {

                return GetValueOrDefault(LastSoughtCacheIdKeyName, LastSoughtCacheIdDefault);
            }
            set
            {
                if (AddOrUpdateValue(LastSoughtCacheIdKeyName, value))
                {
                    Save();
                }
            }
        }

        public CacheProvider LastSoughtCacheProvider
        {
            get
            {
                if (settings.Contains(LastSoughtCacheProviderKeyName))
                {
                    return (CacheProvider)settings[LastSoughtCacheProviderKeyName];
                }
                return CacheProvider.GeocachingSu;
            }
            set
            {
                if (AddOrUpdateValue(LastSoughtCacheProviderKeyName, value))
                {
                    Save();
                }
            }
        }

        public GeoCoordinate LastLocation
        {
            get
            {
                return new GeoCoordinate(GetValueOrDefault(LastLocationLatitudeDefaultKeyName, LastLocationLatitudeDefault),
                    GetValueOrDefault(LastLocationLongitudeDefaultKeyName, LastLocationLongitudeDefault));
            }
            set
            {
                if ((AddOrUpdateValue(LastLocationLatitudeDefaultKeyName, value.Latitude)) &&
                    (AddOrUpdateValue(LastLocationLongitudeDefaultKeyName, value.Longitude)))
                {
                    Save();
                }
            }
        }

        public MapMode MapMode
        {
            get
            {
                return (MapMode)(GetValueOrDefault(MapModeDefaultKeyName, MapModeDefault));
            }
            set
            {
                if (AddOrUpdateValue(MapModeDefaultKeyName, value))
                {
                    Save();
                }
            }
        }

        public bool IsLocationEnabled
        {
            get
            {
                return GetValueOrDefault(IsLocationEnabledKeyName, IsLocationEnabledDefault);
            }
            set
            {
                if (AddOrUpdateValue(IsLocationEnabledKeyName, value))
                {
                    Save();
                    LocationManager.Instance.UpdateIsLocationEnabled(value);
                }
            }
        }

        public bool IsFirstLaunching
        {
            get
            {
                return GetValueOrDefault(IsFirstLaunchingKeyName, IsFirstLaunchingDefault);
            }
            set
            {
                if (AddOrUpdateValue(IsFirstLaunchingKeyName, value))
                {
                    Save();
                }
            }
        }

        public void SetDefaultLastSoughtCacheId()
        {
            if (AddOrUpdateValue(LastSoughtCacheIdKeyName, LastSoughtCacheIdDefault))
            {
                Save();
            }
        }
    }
}
