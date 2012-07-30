using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.ViewModel
{
    public class InfoPivotViewModel : BaseViewModel
    {
        private WebBrowser notebookBrowser;
        private WebBrowser infoBrowser;

        private Visibility noPhotosMessageVisibility = Visibility.Visible;
        private Visibility noNotebookMessageVisibility = Visibility.Visible;
        private Visibility noInfoMessageVisibility = Visibility.Visible;
        private Visibility deleteDialogVisibility = Visibility.Collapsed;

        private ICommand deleteCommand;
        private ICommand cancelCommand;

        private Action afterDeleteAction;
        private bool isPivotEnabled = true;
        public ObservableCollection<Photo> previews;

        public string Info { get; set; }
        public string Notebook { get; set; }
        public Cache Cache { get; set; }

        public ObservableCollection<Photo> Previews 
        { 
            get
            {
                return previews;
            }

            set 
            { 
                previews = value;
                if (value.Count != 0)
                {
                    NoPhotosMessageVisibility = Visibility.Collapsed;
                }
                NotifyPropertyChanged("Previews");
            }
        }

        public Visibility NoPhotosMessageVisibility
        {
            get
            {
                return noPhotosMessageVisibility;
            }
            set
            {
                noPhotosMessageVisibility = value;
                NotifyPropertyChanged("NoPhotosMessageVisibility");
            }
        }

        public Visibility NoNotebookMessageVisibility
        {
            get
            {
                return noNotebookMessageVisibility;
            }
            set
            {
                noNotebookMessageVisibility = value;
                NotifyPropertyChanged("NoNotebookMessageVisibility");
            }
        }

        public Visibility NoInfoMessageVisibility
        {
            get
            {
                return noInfoMessageVisibility;
            }
            set
            {
                noInfoMessageVisibility = value;
                NotifyPropertyChanged("NoInfoMessageVisibility");
            }
        }

        public Visibility DeleteDialogVisibility
        {
            get
            {
                return deleteDialogVisibility;
            }
            set
            {
                deleteDialogVisibility = value;
                NotifyPropertyChanged("DeleteDialogVisibility");
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return deleteCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand;
            }
        }

        public bool IsPivotEnabled
        {
            get
            {
                return isPivotEnabled;
            }
            set
            {
                isPivotEnabled = value;
                NotifyPropertyChanged("IsPivotEnabled");
            }
        }

        public InfoPivotViewModel(Action afterDeleteAction)
        {
            this.afterDeleteAction = afterDeleteAction;

            Previews = new ObservableCollection<Photo>();

            deleteCommand = new ButtonCommand(Delete);
            cancelCommand = new ButtonCommand(Cancel);
        }

        public void LoadNotebookPivotItem(WebBrowser notebookBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);
            this.notebookBrowser = notebookBrowser;

            if ((item != null) && (item.Notebook != null))
            {
                Notebook = item.Notebook;
                NoNotebookMessageVisibility = Visibility.Collapsed;
                notebookBrowser.NavigateToString(item.Notebook);
            }
            else if (Notebook != null)
            {
                LoadAndSaveNotebook(Notebook);
            }
            else
            {
                GeocahingSuApiManager.Instance.DownloadAndProcessNotebook(LoadAndSaveNotebook, Cache.Id);
            }
        }

        public void DownloadAndSaveNotebook()
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);

            if (item.Notebook != null)
            {
                return;
            }

            if (Notebook != null)
            {
                LoadAndSaveNotebook(Notebook);
            }
            else
            {
                GeocahingSuApiManager.Instance.DownloadAndProcessNotebook(LoadAndSaveNotebook, Cache.Id);
            }
        }

        public void LoadDetailsPivotItem(WebBrowser detailsBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);
            infoBrowser = detailsBrowser;

            if ((item != null) && (item.Details != null))
            {
                Info = item.Details;
                NoInfoMessageVisibility = Visibility.Collapsed;
                detailsBrowser.NavigateToString(item.Details);
            }
            else if (Info != null)
            {
                LoadAndSaveCacheInfo(Info);
            }
            else
            {
                GeocahingSuApiManager.Instance.DownloadAndProcessInfo(LoadAndSaveCacheInfo, Cache.Id);
            }
        }

        public void DownloadAndSaveCacheInfo()
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);

            if (item.Details != null)
            {
                return;
            }

            if (Info != null)
            {
                LoadAndSaveCacheInfo(Info);
            }
            else
            {
                GeocahingSuApiManager.Instance.DownloadAndProcessInfo(LoadAndSaveCacheInfo, Cache.Id);
            }
        }

        public void LoadPreviews()
        {
            GeocahingSuApiManager.Instance.LoadPhotos(Cache.Id, LoadPhotos);
        }

        public void DownloadAndSavePhotos()
        {
            GeocahingSuApiManager.Instance.SavePhotos(Cache.Id, SavePhotos);
        }

        public void DeletePhotos()
        {
            GeocahingSuApiManager.Instance.DeletePhotos(Cache.Id);
        }

        private void SavePhotos(ObservableCollection<Photo> photos)
        {
            ProcessPhotos(photos);
        }

        private void LoadAndSaveNotebook(string notebook)
        {
            var db = new CacheDataBase();
            Notebook = notebook;
            if (notebook != "")
            {
                NoNotebookMessageVisibility = Visibility.Collapsed;
            }

            if (db.GetCache(Cache.Id) != null)
            {
                db.UpdateCacheNotebook(notebook, Cache.Id);
            }

            if (notebookBrowser != null)
            {
                notebookBrowser.NavigateToString(notebook);
            }
        }

        private void LoadAndSaveCacheInfo(string info)
        {
            var db = new CacheDataBase();
            Info = info;
            if (info != "")
            {
                NoInfoMessageVisibility = Visibility.Collapsed;
            }

            if (db.GetCache(Cache.Id) != null)
            {
                db.UpdateCacheInfo(info, Cache.Id);
            }

            if (infoBrowser != null)
            {
                infoBrowser.NavigateToString(info);
            }
        }

        public void LoadPhotos(ObservableCollection<Photo> photos)
        {
            ProcessPhotos(photos);
        }

        private void ProcessPhotos(ObservableCollection<Photo> photos)
        {
            if (Previews == null)
            {
                return;
            }

            if (photos == null)
            {
                //todo save photos
                return;
            }

            if (photos.Count == Previews.Count)
            {
                //todo save photos
            }

            if (Previews.Count == 0)
            {
                Previews = photos;
            }
        }

        public void Delete(object p)
        {
            var db = new CacheDataBase();
            db.DeleteCache(Cache.Id);
            DeletePhotos();
            afterDeleteAction();
            DeleteDialogVisibility = Visibility.Collapsed;
            IsPivotEnabled = true;
        }

        public void Cancel(object p)
        {
            DeleteDialogVisibility = Visibility.Collapsed;
            IsPivotEnabled = true;
        }


    }
}
