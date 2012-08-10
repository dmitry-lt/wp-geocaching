using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace WP_Geocaching.Model
{
    public class LocationManager
    {
        private List<ILocationAware> subscribers = new List<ILocationAware>();
        private GeoCoordinateWatcher watcher;
        private bool isLocationEnabled;

        private static LocationManager instance;

        public static LocationManager Instance
        {
            get
            {
                return instance ?? (instance = new LocationManager());
            }
        }

        private LocationManager()
        {
            var settings = new Settings();
            isLocationEnabled = settings.IsLocationEnabled;
            SetNewWatcher(GeoPositionAccuracy.Default);
        }

        private void SetNewWatcher(GeoPositionAccuracy accuracy)
        {
            watcher = new GeoCoordinateWatcher(accuracy)
                          {
                              MovementThreshold = 20
                          };
            watcher.PositionChanged += PositionChanged;
        }

        public void UpdateIsLocationEnabled(bool newValue)
        {
            isLocationEnabled = newValue;

            if (isLocationEnabled)
            {
                StartWatcher();
            }
            else
            {
                StopWatcher();
            }
        }

        public void AddSubscriber(ILocationAware compassView)
        {
            if (compassView != null)
            {
                subscribers.Add(compassView);
            }

            if (compassView.IsNeedHighAccuracy && watcher.DesiredAccuracy == GeoPositionAccuracy.Default)
            {
                StopWatcher();
                SetNewWatcher(GeoPositionAccuracy.High);
                StartWatcher();
            }

            if (subscribers.Count == 1 && isLocationEnabled)
            {
                StartWatcher();
            }
        }

        public void RemoveSubscriber(ILocationAware compassView)
        {
            subscribers.Remove(compassView);

            if (subscribers.Count == 0)
            {
                StopWatcher();
            }

            if (compassView.IsNeedHighAccuracy)
            {
                if (subscribers.Any(c => c.IsNeedHighAccuracy))
                {
                    return;
                }
                SetNewWatcher(GeoPositionAccuracy.Default);
            }
        }

        private void StartWatcher()
        {
            watcher.Start();
        }

        private void StopWatcher()
        {
            watcher.Stop();
        }

        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            foreach (var c in subscribers)
            {
                c.ProcessLocation(e.Position.Location);
            }
        }
    }
}
