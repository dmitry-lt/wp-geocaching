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
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;

namespace WP_Geocaching.ViewModel
{
    public class FavoritesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private CacheClass selectedCache;
        private List<CacheClass> dataSource;

        public List<CacheClass> DataSource
        {
            get
            {
                return this.dataSource;
            }
        }
        public CacheClass SelectedCache
        {
            get { return selectedCache; }
            set
            {
                bool changed = selectedCache != value;
                if (changed)
                {
                    selectedCache = value;
                    OnPropertyChanged(value.Id.ToString());
                }
            }
        }

        public FavoritesViewModel()
        { 
            CacheDataBase db = new CacheDataBase();
            this.dataSource = db.GetCacheList();
            PropertyChanged += ShowDetails;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowDetails(object sender, PropertyChangedEventArgs e)
        {
            NavigationManager.Instance.NavigateToDetails(SelectedCache.Id.ToString());
        }
    }
}

