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
    public class BingMapViewModel : INotifyPropertyChanged
    {
        private int zoom;
        private const int maxCountOfCache = 50;
        private GeoCoordinate mapCenter;
        private IApiManager apiManager;
        private ObservableCollection<CachePushpin> cachePushpinCollection;
        private LocationRect boundingRectangle;
        /*Сообщение,указывающее на большое количество тайников на экране*/
        private String messageIsVisible = "Collapsed";
        public event PropertyChangedEventHandler PropertyChanged;
        
        public BingMapViewModel(IApiManager apiManager)
        {
            BingMapManager manager = new BingMapManager();
            this.MapCenter = manager.DefaulMapCenter;
            this.Zoom = manager.DefaultZoom;
            this.apiManager = apiManager;
            this.CachePushpinCollection = new ObservableCollection<CachePushpin>();
        }
       
        public String MessageIsVisible 
        {
            get 
            {
                return this.messageIsVisible;
            }
          
        }

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
                this.mapCenter = value;
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
        
        public LocationRect BoundingRectangle
        {
            get
            {
                return this.boundingRectangle;
            }
            set
            {
                this.boundingRectangle = value;
                this.GetPushpins();
            }
        }

        private void ProcessCacheList(List<Cache> caches)
        {

           this.CachePushpinCollection.Clear();

           if (caches.Count >= maxCountOfCache){

               if (messageIsVisible.Equals("Collapsed"))
               {
                 messageIsVisible = "Visible";
                 NotifyPropertyChanged("MessageIsVisible");
               }
           }
           else{

                foreach (Cache p in caches)
                {
                    CachePushpin pushpin = new CachePushpin()
                    {
                        Location = p.Location,
                        CacheId = p.Id.ToString(),
                        IconUri = new Enum[2] { p.Type, p.Subtype }
                    };

                    this.CachePushpinCollection.Add(pushpin);
                } 
               
                if (messageIsVisible.Equals("Visible"))
                {
                   messageIsVisible = "Collapsed";
                   NotifyPropertyChanged("MessageIsVisible");
                }
           }
           
        }

        private void GetPushpins()
        {
            this.apiManager.GetCacheList(ProcessCacheList,  BoundingRectangle.East, 
                BoundingRectangle.West, BoundingRectangle.North, BoundingRectangle.South);
        }


        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
