using System.Windows;
using System.Windows.Threading;
using System.Threading;
using System.Device.Location;
using System.Collections.ObjectModel;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Api;

namespace GeocachingPlus.ViewModel
{
    public class BaseMapViewModel : BaseViewModel
    {
        protected IApiManager apiManager;
        protected GeoCoordinate currentLocation;
        protected Visibility undetectedLocationMessageVisibility = Visibility.Collapsed;
        protected double direction;
        protected MapMode mapMode;

        public virtual int Zoom { get; set; }
        public virtual ObservableCollection<CachePushpin> CachePushpins { get; set; }
        public virtual GeoCoordinate MapCenter { get; set; }

        public double Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
                RaisePropertyChanged(() => Direction);
            }
        }

        public Visibility UndetectedLocationMessageVisibility
        {
            get
            {
                return undetectedLocationMessageVisibility;
            }
            set
            {
                undetectedLocationMessageVisibility = value;
                RaisePropertyChanged(() => UndetectedLocationMessageVisibility);
            }
        }

        public MapMode MapMode
        {
            get
            {
                return mapMode;
            }
            set
            {
                mapMode = value;
                RaisePropertyChanged(() => MapMode);
            }
        }

        public void SetMapCenterOnCurrentLocationOrShowMessage(Dispatcher dispatcher)
        {
            if (currentLocation == null)
            {
                UndetectedLocationMessageVisibility = Visibility.Visible;
                var timer = new Timer(state =>
                {
                    var t = (Timer)state;
                    dispatcher.BeginInvoke(() =>
                    {
                        UndetectedLocationMessageVisibility = Visibility.Collapsed;
                    });
                    t.Dispose();
                });
                timer.Change(3000, 0);
            }
            else
            {
                MapCenter = currentLocation;
            }
        }

        protected void UpdateMapMode()
        {
            var settings = new Settings();
            MapMode = settings.MapMode;
        }

        public virtual void UpdateMapProperties()
        {
            UpdateMapMode();
        }
    }
}
