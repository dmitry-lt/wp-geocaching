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
        private string dialogVisibility;
        private bool isEnabled;
        private ChooseorDeleteDialogViewModel dialogContent;

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
                selectedCheckpoint = value;
                if (value != null)
                {
                    ShowMakeActiveorDeleteDialogDialog();
                }
            }
        }
        public string DialogVisibility
        {
            get { return dialogVisibility; }
            set
            {
                bool changed = dialogVisibility != value;
                if (changed)
                {
                    dialogVisibility = value;
                    OnPropertyChanged("DialogVisibility");
                }
            }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                bool changed = isEnabled != value;
                if (changed)
                {
                    isEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }
        public ChooseorDeleteDialogViewModel DialogContent
        {
            get { return dialogContent; }
            set
            {
                bool changed = dialogContent != value;
                if (changed)
                {
                    dialogContent = value;
                    OnPropertyChanged("DialogContent");
                }
            }
        }

        public CheckpointsViewModel()
        {
            DialogVisibility = "Collapsed";
            IsEnabled = true;
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

        private void ShowMakeActiveorDeleteDialogDialog()
        {
            DialogVisibility = "Visible";
            DialogContent = new ChooseorDeleteDialogViewModel(SelectedCheckpoint, CloseMakeActiveorDeleteDialogDialog);
            IsEnabled = false;
        }
        public void CloseMakeActiveorDeleteDialogDialog()
        {
            DialogVisibility = "Collapsed";
            IsEnabled = true;
            UpdateDataSource();
        }
    }
}

