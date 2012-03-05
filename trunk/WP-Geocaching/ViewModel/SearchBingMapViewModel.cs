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
using System.Xml;
using System.Text;
using System.IO;
using System.Xml.Linq;
using WP_Geocaching.Model;
using System.ComponentModel;
using System.Collections.Generic;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;

namespace WP_Geocaching.ViewModel
{
    public class SearchBingMapViewModel : INotifyPropertyChanged
    {
        private int zoom;
        public event PropertyChangedEventHandler PropertyChanged;
        private GeoCoordinate mapCenter;
        private IApiManager apiManager;
        private ObservableCollection<CachePushpin> cachePushpinCollection;
        private Cache cache;

        public int Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                this.zoom = value;
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
                bool changed = mapCenter != value;
                if (changed)
                {
                    mapCenter = value;
                    OnPropertyChanged("MapCenter");
                }
            }
        }

        public Cache Cache
        {
            get
            {
                return this.cache;
            }
            set
            {
                cache = value;               
                if (cache != null)
                {
                    MapCenter = value.Location;
                    CachePushpin pushpin = new CachePushpin()
                            {
                                Location = cache.Location,
                                CacheId = cache.Id.ToString(),
                                IconUri = new Uri(cache.Type.ToString() + cache.Subtype.ToString(), UriKind.Relative),
                            };
                    CachePushpinCollection.Add(pushpin);
                }
            }
        }

        public ObservableCollection<CachePushpin> CachePushpinCollection
        {
            get
            {
                return this.cachePushpinCollection;
            }
            set
            {
                this.cachePushpinCollection = value;
            }
        }

        public SearchBingMapViewModel(IApiManager apiManager)
        {
            BingMapManager manager = new BingMapManager();
            this.zoom = manager.DefaultZoom;
            this.apiManager = apiManager;
            this.CachePushpinCollection = new ObservableCollection<CachePushpin>();
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
