using System.Collections.Generic;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;

namespace WP_Geocaching.ViewModel
{
    public class CheckpointsViewModel : BaseViewModel
    {
        private int cacheId;
        private ListCacheItem selectedCheckpoint;
        private List<ListCacheItem> dataSource;
        private string dialogVisibility;
        private bool isListEnabled;
        private ChooseoOrDeleteDialogViewModel dialogContent;

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
                    NotifyPropertyChanged("DialogVisibility");
                }
            }
        }
        public bool IsListEnabled
        {
            get { return isListEnabled; }
            set
            {
                bool changed = isListEnabled != value;
                if (changed)
                {
                    isListEnabled = value;
                    NotifyPropertyChanged("IsListEnabled");
                }
            }
        }
        public ChooseoOrDeleteDialogViewModel DialogContent
        {
            get { return dialogContent; }
            set
            {
                bool changed = dialogContent != value;
                if (changed)
                {
                    dialogContent = value;
                    NotifyPropertyChanged("DialogContent");
                }
            }
        }

        public CheckpointsViewModel()
        {
            DialogVisibility = "Collapsed";
            IsListEnabled = true;
            cacheId = MapManager.Instance.CacheId;
            UpdateDataSource();
        }

        public void UpdateDataSource()
        {
            CacheDataBase db = new CacheDataBase();
            List<DbCheckpointsItem> dbCheckpointsList = db.GetCheckpointsByCacheId(cacheId);
            List<ListCacheItem> newDataSource = new List<ListCacheItem>();
            newDataSource.Add(new ListCacheItem(db.GetCache(cacheId)));
            foreach (DbCheckpointsItem c in dbCheckpointsList)
            {
                newDataSource.Add(new ListCacheItem(c));
            }
            this.DataSource = newDataSource;
        }

        public void CloseMakeActiveorDeleteDialogDialog()
        {
            DialogVisibility = "Collapsed";
            IsListEnabled = true;
            UpdateDataSource();
        }

        private void ShowMakeActiveorDeleteDialogDialog()
        {
            DialogVisibility = "Visible";
            DialogContent = new ChooseoOrDeleteDialogViewModel(cacheId, SelectedCheckpoint, CloseMakeActiveorDeleteDialogDialog);
            IsListEnabled = false;
        }
    }
}

