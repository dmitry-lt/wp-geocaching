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
        private DbCacheItem selectedCache;
        private List<FavoriteClass> dataSource;

        public List<FavoriteClass> DataSource
        {
            get
            {
                return this.dataSource;
            }
        }
        public DbCacheItem SelectedCache
        {
            get { return selectedCache; }
            set
            {
                bool changed = selectedCache != value;
                if (changed)
                {
                    selectedCache = value;
                    OnPropertyChanged("SelectedCache");
                    if (value != null)
                    {
                        ShowDetails(value.Id.ToString());
                    }
                }
            }
        }

        public FavoritesViewModel()
        { 
            this.dataSource = GetDataSource();          
        }

        private List<FavoriteClass> GetDataSource()
        {
            CacheDataBase db = new CacheDataBase();
            List<DbCacheItem> dbCacheList = db.GetCacheList();
            List<FavoriteClass> dataSource = new List<FavoriteClass>();
            foreach (DbCacheItem c in dbCacheList)
            {
                dataSource.Add(new FavoriteClass(c));
            }
            return dataSource;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowDetails(string cacheId)
        {
            NavigationManager.Instance.NavigateToDetails(cacheId);
            SelectedCache = null;
        }
    }
}

