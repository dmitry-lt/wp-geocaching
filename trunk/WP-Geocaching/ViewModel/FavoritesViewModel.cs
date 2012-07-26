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
    public class FavoritesViewModel : BaseViewModel
    {
        private ListCacheItem selectedCache;
        private List<ListCacheItem> dataSource;

        public List<ListCacheItem> DataSource
        {
            get
            {
                return this.dataSource;
            }
            private set
            {
                bool changed = dataSource != value;
                if (changed)
                {
                    dataSource = value;
                    NotifyPropertyChanged("DataSource");
                }
            }
        }
        public ListCacheItem SelectedCache
        {
            get { return selectedCache; }
            set
            {
                selectedCache = value;
                if (value != null)
                {
                    ShowDetails(value.Id.ToString());
                }
            }
        }

        public FavoritesViewModel()
        {
            UpdateDataSource();
        }

        public void UpdateDataSource()
        {
            DataSource = GetDataSource();
        }

        private List<ListCacheItem> GetDataSource()
        {
            CacheDataBase db = new CacheDataBase();
            List<DbCacheItem> dbCacheList = db.GetCacheList();
            List<ListCacheItem> dataSource = new List<ListCacheItem>();
            foreach (DbCacheItem c in dbCacheList)
            {
                dataSource.Add(new ListCacheItem(c));
            }
            dataSource.Sort((a, b) => b.DateAdded.CompareTo(a.DateAdded));
            return dataSource;
        }

        private void ShowDetails(string cacheId)
        {
            NavigationManager.Instance.NavigateToInfoPivot(cacheId);
        }
    }
}

