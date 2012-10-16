﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model.Dialogs;

namespace WP_Geocaching.ViewModel
{
    public class CheckpointsViewModel : BaseViewModel
    {
        private string cacheId;
        private ListCacheItem selectedCheckpoint;
        private ChooseOrDeleteDialog chooseOrDeleteDialog;
        private List<ListCacheItem> dataSource;
        private Dispatcher dispatcher;

        public List<ListCacheItem> DataSource
        {
            get
            {
                return dataSource;
            }
            private set
            {
                dataSource = value;
                NotifyPropertyChanged("DataSource");
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
                    ShowMakeActiveOrDeleteDialogDialog();
                }
            }
        }

        public CheckpointsViewModel(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            cacheId = MapManager.Instance.CacheId;
            chooseOrDeleteDialog = new ChooseOrDeleteDialog(cacheId, CloseMakeActiveOrDeleteDialogDialog, dispatcher);
            UpdateDataSource();
        }

        public void UpdateDataSource()
        {
            var db = new CacheDataBase();
            var dbCheckpointsList = db.GetCheckpointsByCacheId(cacheId);
            var newDataSource = new List<ListCacheItem>();
            newDataSource.Add(new ListCacheItem(db.GetCache(cacheId)));
            newDataSource.AddRange(dbCheckpointsList.Select(c => new ListCacheItem(c)));
            DataSource = newDataSource;
        }

        public void CloseMakeActiveOrDeleteDialogDialog()
        {
            UpdateDataSource();
        }

        private void ShowMakeActiveOrDeleteDialogDialog()
        {
            chooseOrDeleteDialog.Show(SelectedCheckpoint);
        }
    }
}

