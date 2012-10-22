using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace GeocachingPlus.Model
{
    public class LocationManager
    {
        private readonly List<ILocationAware> _subscribers = new List<ILocationAware>();
        private GeoCoordinateWatcher _watcher;
        private bool _isLocationEnabled;

        private static LocationManager _instance;

        public static LocationManager Instance
        {
            get
            {
                return _instance ?? (_instance = new LocationManager());
            }
        }

        private LocationManager()
        {
            var settings = new Settings();
            _isLocationEnabled = settings.IsLocationEnabled;
            SetNewWatcher(GeoPositionAccuracy.Default);
        }

        private void SetNewWatcher(GeoPositionAccuracy accuracy)
        {
            var movementThreshold = accuracy == GeoPositionAccuracy.High ? 1 : 20;
            _watcher = new GeoCoordinateWatcher(accuracy)
            {
                MovementThreshold = movementThreshold
            };
            _watcher.PositionChanged += PositionChanged;
        }

        public void UpdateIsLocationEnabled(bool newValue)
        {
            _isLocationEnabled = newValue;

            if (_isLocationEnabled)
            {
                StartWatcher();
            }
            else
            {
                StopWatcher();
            }
        }

        public void AddSubscriber(ILocationAware locationAware)
        {
            if (locationAware != null)
            {
                if (!_subscribers.Contains(locationAware))
                {
                    _subscribers.Add(locationAware);
                }

                if (locationAware.IsNeedHighAccuracy && _watcher.DesiredAccuracy == GeoPositionAccuracy.Default)
                {
                    StopWatcher();
                    SetNewWatcher(GeoPositionAccuracy.High);
                    StartWatcher();
                }
            }

            if (_subscribers.Count == 1 && _isLocationEnabled)
            {
                StartWatcher();
            }
        }

        public void RemoveSubscriber(ILocationAware locationAware)
        {
            if (_subscribers.Contains(locationAware))
            {
                _subscribers.Remove(locationAware);
            }

            if (_subscribers.Count == 0)
            {
                StopWatcher();
            }

            if (locationAware.IsNeedHighAccuracy)
            {
                if (_subscribers.Any(c => c.IsNeedHighAccuracy))
                {
                    return;
                }
                SetNewWatcher(GeoPositionAccuracy.Default);
            }
        }

        private void StartWatcher()
        {
            _watcher.Start();
        }

        private void StopWatcher()
        {
            _watcher.Stop();
        }

        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            foreach (var c in _subscribers)
            {
                c.ProcessLocation(e.Position.Location);
            }
        }
    }
}
