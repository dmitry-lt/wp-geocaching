using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Device.Location;

namespace WP_Geocaching.ViewModel
{
    public class BaseMapViewModel : BaseViewModel
    {
        protected GeoCoordinate mapCenter;
        protected GeoCoordinate currentLocation;
        protected Visibility undetectedLocationMessageVisibility = Visibility.Collapsed;

        public Visibility UndetectedLocationMessageVisibility
        {
            get
            {
                return this.undetectedLocationMessageVisibility;
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
                return this.mapCenter;
            }
            set
            {
                mapCenter = value;
                NotifyPropertyChanged("MapCenter");
            }
        }

        public void SetMapCenterOnCurrentLocationOrShowMessage(Dispatcher dispatcher)
        {
            if (currentLocation == null)
            {
                UndetectedLocationMessageVisibility = Visibility.Visible;
                var timer = new Timer((state) =>
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
    }
}
