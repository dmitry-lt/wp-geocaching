using System;
using System.IO.IsolatedStorage;
using System.Device.Location;
using GeocachingPlus.Model.Api;

namespace GeocachingPlus.Model
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
        private const string LatestSoughtCacheIdKeyName = "LatestSoughtCacheId";
        private const string LatestSoughtCacheProviderKeyName = "LatestSoughtCacheProvider";
        private const string LatestSearchLocationLatitudeDefaultKeyName = "LatestSearchLocationLatitude";
        private const string LatestSearchLocationLongitudeDefaultKeyName = "LatestSearchLocationLongitude";
        private const string LatestChooseLocationLatitudeDefaultKeyName = "LatestChooseLocationLatitude";
        private const string LatestChooseLocationLongitudeDefaultKeyName = "LatestChooseLocationLongitude";
        private const string LatestChooseZoomDefaultKeyName = "LatestChooseZoom";
        private const string MapModeDefaultKeyName = "MapMode";
        private const string IsLocationEnabledKeyName = "IsLocationEnabled";
        private const string IsFirstLaunchingKeyName = "IsFirstLaunching";
        private const string IsOpenCachingComEnabledKeyName = "IsOpenCachingComEnabled";
        private const string IsGeocachingSuEnabledKeyName = "IsGeocachingSuEnabled";
        private const string IsGeocachingComEnabledKeyName = "IsGeocachingComEnabled";

        // The default value of our settings
        private const string LatestSoughtCacheIdDefault = "";
        private const CacheProvider LatestSoughtCacheProviderDefault = CacheProvider.GeocachingSu;
        private const double LatestLocationLatitudeDefault = 59.879904;
        private const double LatestLocationLongitudeDefault = 29.828674;
        private const int MapModeDefault = (int)MapMode.Road;
        private const bool IsLocationEnabledDefault = true;
        private const bool IsFirstLaunchingDefault = true;
        private const int LatestChooseZoomDefault = 13;

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


        public string LatestSoughtCacheId
        {
            get
            {

                return GetValueOrDefault(LatestSoughtCacheIdKeyName, LatestSoughtCacheIdDefault);
            }
            set
            {
                if (AddOrUpdateValue(LatestSoughtCacheIdKeyName, value))
                {
                    Save();
                }
            }
        }

        public CacheProvider LatestSoughtCacheProvider
        {
            get
            {
                if (_settings.Contains(LatestSoughtCacheProviderKeyName))
                {
                    return (CacheProvider)_settings[LatestSoughtCacheProviderKeyName];
                }
                return LatestSoughtCacheProviderDefault;
            }
            set
            {
                if (AddOrUpdateValue(LatestSoughtCacheProviderKeyName, value))
                {
                    Save();
                }
            }
        }

        public GeoCoordinate LatestSearchLocation
        {
            get
            {
                return new GeoCoordinate(GetValueOrDefault(LatestSearchLocationLatitudeDefaultKeyName, LatestLocationLatitudeDefault),
                    GetValueOrDefault(LatestSearchLocationLongitudeDefaultKeyName, LatestLocationLongitudeDefault));
            }
            set
            {
                if ((AddOrUpdateValue(LatestSearchLocationLatitudeDefaultKeyName, value.Latitude)) &&
                    (AddOrUpdateValue(LatestSearchLocationLongitudeDefaultKeyName, value.Longitude)))
                {
                    Save();
                }
            }
        }

        public GeoCoordinate LatestChooseLocation
        {
            get
            {
                return new GeoCoordinate(GetValueOrDefault(LatestChooseLocationLatitudeDefaultKeyName, LatestLocationLatitudeDefault),
                    GetValueOrDefault(LatestChooseLocationLongitudeDefaultKeyName, LatestLocationLongitudeDefault));
            }
            set
            {
                if ((AddOrUpdateValue(LatestChooseLocationLatitudeDefaultKeyName, value.Latitude)) &&
                    (AddOrUpdateValue(LatestChooseLocationLongitudeDefaultKeyName, value.Longitude)))
                {
                    Save();
                }
            }
        }

        public int LatestChooseZoom
        {
            get
            {
                return GetValueOrDefault(LatestChooseZoomDefaultKeyName, LatestChooseZoomDefault);
            }
            set
            {
                if (AddOrUpdateValue(LatestChooseZoomDefaultKeyName, value))
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

        public void SetDefaultLastSoughtCache()
        {
            if (AddOrUpdateValue(LatestSoughtCacheIdKeyName, LatestSoughtCacheIdDefault))
            {
                Save();
            }
            if (AddOrUpdateValue(LatestSoughtCacheProviderKeyName, LatestSoughtCacheProviderDefault))
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

        public bool IsGeocachingComEnabled
        {
            get
            {
                return GetValueOrDefault(IsGeocachingComEnabledKeyName, true);
            }
            set
            {
                if (AddOrUpdateValue(IsGeocachingComEnabledKeyName, value))
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

                case CacheProvider.GeocachingCom:
                    return IsGeocachingComEnabled;

                default:
                    throw new ArgumentException();

            }
        }

    }
}
