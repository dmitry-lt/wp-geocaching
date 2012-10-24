﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using GeocachingPlus.Model;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.Model.Dialogs;

namespace GeocachingPlus.ViewModel
{
    public class CheckpointsViewModel : BaseViewModel
    {
        private Cache cache;
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
                RaisePropertyChanged(() => DataSource);
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
            cache = MapManager.Instance.Cache;
            chooseOrDeleteDialog = new ChooseOrDeleteDialog(cache, CloseMakeActiveOrDeleteDialogDialog, dispatcher);
            UpdateDataSource();
        }

        public void UpdateDataSource()
        {
            var db = new CacheDataBase();
            var dbCheckpointsList = db.GetCheckpointsByCache(cache);
            var newDataSource = new List<ListCacheItem>();
            newDataSource.Add(new ListCacheItem(DbConvert.ToDbCacheItem(cache)));
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
