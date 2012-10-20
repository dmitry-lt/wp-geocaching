using System;
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
        private readonly IsolatedStorageSettings _settings;

        // The isolated storage key names of our settings
        private const string LastSoughtCacheIdKeyName = "LastSoughtCacheId";
        private const string LastSoughtCacheProviderKeyName = "LastSoughtCacheProvider";
        private const string LastSearchLocationLatitudeDefaultKeyName = "LastSearchLocationLatitude";
        private const string LastSearchLocationLongitudeDefaultKeyName = "LastSearchLocationLongitude";
        private const string MapModeDefaultKeyName = "MapMode";
        private const string IsLocationEnabledKeyName = "IsLocationEnabled";
        private const string IsFirstLaunchingKeyName = "IsFirstLaunching";
        private const string IsOpenCachingComEnabledKeyName = "IsOpenCachingComEnabled";
        private const string IsGeocachingSuEnabledKeyName = "IsGeocachingSuEnabled";

        // The default value of our settings
        private const string LastSoughtCacheIdDefault = "";
        private const double LastLocationLatitudeDefault = 59.879904;
        private const double LastLocationLongitudeDefault = 29.828674;
        private const int MapModeDefault = (int)MapMode.Road;
        private const bool IsLocationEnabledDefault = true;
        private const bool IsFirstLaunchingDefault = true;

        public Settings()
        {
            _settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;

            if (_settings.Contains(key))
            {
                if (_settings[key] != value)
                {
                    _settings[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                _settings.Add(key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            if (_settings.Contains(key))
            {
                value = (T)_settings[key];
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }

        public void Save()
        {
            _settings.Save();
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
                if (_settings.Contains(LastSoughtCacheProviderKeyName))
                {
                    return (CacheProvider)_settings[LastSoughtCacheProviderKeyName];
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

        public GeoCoordinate LastSearchLocation
        {
            get
            {
                return new GeoCoordinate(GetValueOrDefault(LastSearchLocationLatitudeDefaultKeyName, LastLocationLatitudeDefault),
                    GetValueOrDefault(LastSearchLocationLongitudeDefaultKeyName, LastLocationLongitudeDefault));
            }
            set
            {
                if ((AddOrUpdateValue(LastSearchLocationLatitudeDefaultKeyName, value.Latitude)) &&
                    (AddOrUpdateValue(LastSearchLocationLongitudeDefaultKeyName, value.Longitude)))
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

        public bool IsOpenCachingComEnabled
        {
            get
            {
                return GetValueOrDefault(IsOpenCachingComEnabledKeyName, true);
            }
            set
            {
                if (AddOrUpdateValue(IsOpenCachingComEnabledKeyName, value))
                {
                    Save();
                }
            }
        }

        public bool IsGeocachingSuEnabled
        {
            get
            {
                return GetValueOrDefault(IsGeocachingSuEnabledKeyName, true);
            }
            set
            {
                if (AddOrUpdateValue(IsGeocachingSuEnabledKeyName, value))
                {
                    Save();
                }
            }
        }

        public bool IsCacheProviderEnabled(CacheProvider cacheProvider)
        {
            switch (cacheProvider)
            {
                case CacheProvider.GeocachingSu:
                    return IsGeocachingSuEnabled;

                case CacheProvider.OpenCachingCom:
                    return IsOpenCachingComEnabled;

                default:
                    throw new ArgumentException();

            }
        }

    }
}
