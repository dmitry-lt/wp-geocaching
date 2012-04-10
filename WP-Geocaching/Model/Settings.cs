using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.Device.Location;

namespace WP_Geocaching.Model
{
    public class Settings
    {
        IsolatedStorageSettings settings;

        // The isolated storage key names of our settings
        const string LastSoughtCacheIdKeyName = "LastSoughtCacheId";
        const string LastLocationLatitudeDefaultKeyName = "LastLocationLatitude";
        const string LastLocationLongitudeDefaultKeyName = "LastLocationLongitude";

        // The default value of our settings
        const int LastSoughtCacheIdDefault = -1;
        const double LastLocationLatitudeDefault = 59.879904;
        const double LastLocationLongitudeDefault = 29.828674;

        public Settings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            if (settings.Contains(Key))
            {
                if (settings[Key] != value)
                {
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
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


        public int LastSoughtCacheId
        {
            get
            {
                return GetValueOrDefault<int>(LastSoughtCacheIdKeyName, LastSoughtCacheIdDefault);
            }
            set
            {
                if (AddOrUpdateValue(LastSoughtCacheIdKeyName, value))
                {
                    Save();
                }
            }
        }

        public GeoCoordinate LastLocation
        {
            get
            {
                return new GeoCoordinate (GetValueOrDefault<double>(LastLocationLatitudeDefaultKeyName, LastLocationLatitudeDefault),
                    GetValueOrDefault<double>(LastLocationLongitudeDefaultKeyName, LastLocationLongitudeDefault));
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

        public void SetDefaultLastSoughtCacheId ()
        {
            if (AddOrUpdateValue(LastSoughtCacheIdKeyName, LastSoughtCacheIdDefault))
                {
                    Save();
                }
        }
    }
}
