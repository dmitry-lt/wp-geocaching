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
using System.Collections.Generic;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;
using System.ComponentModel;

namespace WP_Geocaching.ViewModel
{
    public class CheckpointsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ListCacheItem selectedCheckpoint;
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
                    OnPropertyChanged("DataSource");
                }
            
            }
        }
        public ListCacheItem SelectedCheckpoint
        {
            get { return selectedCheckpoint; }
            set
            {
                bool changed = selectedCheckpoint != value;
                if (changed)
                {
                    selectedCheckpoint = value;
                    OnPropertyChanged("SelectedCheckpoint");
                }
            }
        }

        public CheckpointsViewModel()
        { 
            UpdateDataSource();          
        }

        public void UpdateDataSource()
        {
            CacheDataBase db = new CacheDataBase();
            List<DbCheckpointsItem> dbCheckpointsList = db.GetCheckpointsbyCacheId(MapManager.Instance.CacheId);
            List<ListCacheItem> newDataSource = new List<ListCacheItem>();
            newDataSource.Add(new ListCacheItem(db.GetCache(MapManager.Instance.CacheId)));
            foreach (DbCheckpointsItem c in dbCheckpointsList)
            {
                newDataSource.Add(new ListCacheItem(c));
            }
            this.DataSource = newDataSource;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

