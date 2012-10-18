using System.Windows;
using System.Windows.Threading;
using System.Threading;
using System.Device.Location;
using System.Collections.ObjectModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.Api;

namespace WP_Geocaching.ViewModel
{
    public class BaseMapViewModel : BaseViewModel
    {
        protected GeoCoordinate mapCenter;
        protected IApiManager apiManager;
        protected GeoCoordinate currentLocation;
        protected Visibility undetectedLocationMessageVisibility = Visibility.Collapsed;
        protected double direction;
        protected MapMode mapMode;
        protected Settings settings;

        public virtual int Zoom { get; set; }
        public virtual ObservableCollection<CachePushpin> CachePushpins { get; set; }

        public double Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
                NotifyPropertyChanged("Direction");
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
                NotifyPropertyChanged("UndetectedLocationMessageVisibility");
            }
        }

        public GeoCoordinate MapCenter
        {
            get
            {
                return mapCenter;
            }
            set
            {
                mapCenter = value;
                NotifyPropertyChanged("MapCenter");
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
                NotifyPropertyChanged("MapMode");
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
            settings = new Settings();
            MapMode = settings.MapMode;
        }

        public virtual void UpdateMapProperties()
        {
            UpdateMapMode();
        }
    }
}
